using System.Numerics;

namespace AoC.Days;

internal class GpuParticleSwarmer
{
    private List<GpuParticle> Particles { get; } = [];

    // Simulation Data
    private const int TickCount = 5; // Simulation ticks per infinite particle check.
    private List<GpuParticle> LiveParticles { get; set; } = [];
    private int InfiniteParticleCount { get; set; } = 0;

    public void ReadParticleData(string[] particleDatas)
    {
        Particles.Clear();
        int index = 0;
        foreach (string particleData in particleDatas)
        {
            string[] leftSplit = particleData.Split('<');
            string[] positionSplit = leftSplit[1].Split('>');
            string[] velocitySplit = leftSplit[2].Split('>');
            string[] accelerationSplit = leftSplit[3].Split('>');

            string[] positionStrings = positionSplit[0].Split(',');
            string[] velocityStrings = velocitySplit[0].Split(',');
            string[] accelerationStrings = accelerationSplit[0].Split(',');

            int[] position = new int[3];
            int[] velocity = new int[3];
            int[] acceleration = new int[3];
            for (int i = 0; i < 3; i++)
            {
                position[i] = int.Parse(positionStrings[i]);
                velocity[i] = int.Parse(velocityStrings[i]);
                acceleration[i] = int.Parse(accelerationStrings[i]);
            }
            Particles.Add(new(position, velocity, acceleration, index++));
        }
    }

    public string GetClosestLongTermToOrigin()
    {
        List<GpuParticle> accelerationCandidates = [];
        // Find the acceleration values with the lowest manhattan distance to 0
        int minimumAccelerationTotal = int.MaxValue;
        foreach (GpuParticle particle in Particles)
        {
            int accelTotal = Math.Abs(particle.Acceleration[0]) + Math.Abs(particle.Acceleration[1]) + Math.Abs(particle.Acceleration[2]);
            if (accelTotal > minimumAccelerationTotal)
            {
                continue;
            }

            if (accelTotal < minimumAccelerationTotal) 
            {
                accelerationCandidates.Clear();
                minimumAccelerationTotal = accelTotal;
            }
            accelerationCandidates.Add(particle);

            // Calculate new velocity and position values for each particle, respective of the direction of acceleration (this is a tiebreaker measure for distance to origin).
            for (int axis = 0; axis < 3; axis++)
            {
                if (particle.Acceleration[axis] < 0)
                {
                    particle.Velocity[axis] *= -1;
                    particle.Position[axis] *= -1;
                }

                if (particle.Acceleration[axis] == 0)
                {
                    particle.Velocity[axis] = Math.Abs(particle.Velocity[axis]);
                    particle.Position[axis] = Math.Abs(particle.Position[axis]);
                }
            }
        }

        // Now, of these minimum accelerating particles, find the particles with the lowest manhattan distance for velocity.
        List<GpuParticle> velocityCandidates = [];
        int minimumVelocityTotal = int.MaxValue;
        foreach (GpuParticle particle in accelerationCandidates)
        {
            int velocityTotal = particle.Velocity.Sum();
            if (velocityTotal > minimumVelocityTotal)
            {
                continue;
            }

            if (velocityTotal < minimumVelocityTotal)
            {
                velocityCandidates.Clear();
                minimumVelocityTotal = velocityTotal;
            }
            velocityCandidates.Add(particle);
        }

        // Now, of these minimum accelerating, minimum start velocity particles, find the particles with the lowest start manhattan distance.
        List<GpuParticle> positionCandidates = [];
        int minimumPositionTotal = int.MaxValue;
        foreach (GpuParticle particle in velocityCandidates)
        {
            int positionTotal = particle.Position.Sum();
            if (positionTotal > minimumPositionTotal)
            {
                continue;
            }

            if (positionTotal < minimumPositionTotal)
            {
                positionCandidates.Clear();
                minimumPositionTotal = positionTotal;
            }
            positionCandidates.Add(particle);
        }

        return positionCandidates.First().Id.ToString();
    }

    public string GetCountInfiniteParticles()
    {
        /* Method:
         * Simulate some reasonable number of ticks, maybe 500.
         * Remove any collisions that occur during the ticks, do this by:
         * making a Dictionary of positions covered for a given tick (that gives you the particle that first covered this position)
         * If a particle lands on a position contained in the dictionary, add both the dictionary particle and this particle to the pool of collided particles
         * At the end of a tick, take the set difference between the collided particles and 'live' particles to get the new set of 'live' particles for the next tick
         * Clear the covered positions dictionary.
         * 
         * Once your reasonable number of ticks has been run, then try checking which particles are considered infinite (will surely never collide with another particle)
         * Remove these from the 'live' particles, as we dont need to simulate them anymore.
         * Add them to the infinite particles set (or just increment counter for each infinite one found)
         * 
         * Then go back to simulating ticks for those ones that may still collide.
         * Rinse repeat until the 'live' pool is empty.
         */

        /* But how do we determine if a particle will surely never collide with any other particle?
         * Well we check for each other particle, will it surely never collide with that particle, and if thats true for all other particles, then we've proven it.
         * But how do we do it for a pair of particles?...
         * 
         * The formula for a particle's position p on an axis given x ticks have passed is (v and a are that axis' velocity and acceleration respectively):
         * p + (v+a) + (v+2a) + (v+3a) + ... (v+xa)) =
         * p + xv + (a + 2a + ... + xa) =
         * p + xv + a(1 + 2 + 3 + ... + x) =
         * p + xv + a((x+1)x/2)
         * 
         * So we can solve for two different particles, if these formulas are equal to eachother...
         * p1 - p2 + (v1 - v2)x + a1((x+1)x/2) - a2((x+1)x/2) = 0
         * (p1 - p2) + (v1 - v2)x + ((a1 - a2)/2)x  + ((a1 - a2)/2)x^2
         * This can be solved with the quadratic formula (as its a quadratic equation), and if any positive roots exist that means a collision will occur, atleast for the position at time x on this axis.
         * a = (a1 - a2) / 2
         * b = (v1 - v2) + ((a1 - a2) / 2) = (v1 - v2) + a
         * c = (p1 - p2)
         * 
         * So we invert it: if for any axis, there are no positive roots, then the particles will surely never collide.
         */

        // Step zero: Take all the particles and put a reference to each into the LiveParticles list.
        LiveParticles.AddRange(Particles);

        // First, determine if there are any collisions at start state, destroy those particles.
        HashSet<GpuParticle> collisions = DetermineCollisions(LiveParticles);
        LiveParticles = [.. LiveParticles.Except(collisions)];

        // Secondly, begin the tick-simulation and infinite particle checks until there are no more 'live' particles.
        while (LiveParticles.Count > 1)
        {
            // Run a suitable amount of ticks.
            for (int tick = 0; tick < TickCount; tick++)
            {
                RunOneTick(LiveParticles);
                // Check for collisions after each tick, and destroy those particles.
                collisions = DetermineCollisions(LiveParticles);
                LiveParticles.RemoveAll(p => collisions.Contains(p));

                // If there is only 1 or 0 live particles left, we can stop simulating (no collision is possible)
                if (LiveParticles.Count <= 1)
                {
                    break;
                }
            }

            // If there is 1 or 0 live particles left, break the loop early.
            if (LiveParticles.Count <= 1)
            {
                break;
            }

            // Use quadratic formula to compare particles and determine if any particles cannot ever collide with all others.
            HashSet<GpuParticle> collidables = [];
            HashSet<GpuParticle> infinites = [];

            // The method here is to compare every single pair of particles, and after a particle has been fully compared, we check if it is in the (potentially) collidables set, and if not, then its infinite.
            for (int indexOne = 0; indexOne < LiveParticles.Count - 1; indexOne++)
            {
                GpuParticle particleOne = LiveParticles[indexOne];
                for (int indexTwo = indexOne + 1; indexTwo < LiveParticles.Count; indexTwo++)
                {
                    GpuParticle particleTwo = LiveParticles[indexTwo];

                    if (!SurelyWontCollide(particleOne, particleTwo))
                    {
                        collidables.Add(particleOne);
                        collidables.Add(particleTwo);
                    }
                }

                if (!collidables.Contains(particleOne))
                {
                    infinites.Add(particleOne);
                }
            }

            // For any infinite particle, we remove it from the live particle list, because we dont need to simulate it anymore.
            LiveParticles.RemoveAll(p => infinites.Contains(p));
            InfiniteParticleCount += infinites.Count;
        }

        InfiniteParticleCount += LiveParticles.Count;
        LiveParticles.Clear();
        return InfiniteParticleCount.ToString();
    }

    private HashSet<GpuParticle> DetermineCollisions(List<GpuParticle> particleList)
    {
        // Compare every pair of particles, and if their positions are equal, add both to the collided particles set.
        HashSet<GpuParticle> collidedParticles = [];
        for (int indexOne = 0; indexOne < particleList.Count - 1; indexOne++)
        {
            GpuParticle particleOne = particleList[indexOne];
            for (int indexTwo = indexOne + 1; indexTwo < particleList.Count; indexTwo++)
            {
                GpuParticle particleTwo = particleList[indexTwo];
                if (particleOne.Position.SequenceEqual(particleTwo.Position))
                {
                    collidedParticles.Add(particleOne);
                    collidedParticles.Add(particleTwo);
                }
            }
        }
        return collidedParticles;
    }

    private void RunOneTick(List<GpuParticle> particles)
    {
        foreach(GpuParticle particle in particles)
        {
            for (int axis = 0; axis < 3; axis++)
            {
                particle.Velocity[axis] += particle.Acceleration[axis];
                particle.Position[axis] += particle.Velocity[axis];
            }
        }
    }

    // I could make this better by turning this check into pure integer maths (rather than floating point) but its accurate for this use-case right now.
    private bool SurelyWontCollide(GpuParticle particleOne, GpuParticle particleTwo)
    {
        // Two particles will never collide if the quadratic equation for their position on any of the 3 axes being equal has no non-negative (cant be in the past), integer (we run on ticks, not continuous time) roots.
        // root = (-b +- sqrt((b^2) - 4ac)) / 2a

        for (int axis = 0; axis < 3; axis++)
        {
            double a1 = (double)particleOne.Acceleration[axis];
            double a2 = (double)particleTwo.Acceleration[axis];
            double v1 = (double)particleOne.Velocity[axis];
            double v2 = (double)particleTwo.Velocity[axis];
            double p1 = (double)particleOne.Position[axis];
            double p2 = (double)particleTwo.Position[axis];

            double a = (a1 - a2) / 2f;

            if (a == 0)
            {
                return SurelyWontLinearCollide1D(p1, v1, p2, v2);
            }

            double b = (v1 - v2) + a;
            double c = (p1 - p2);

            double discriminant = (b * b) - (4 * a * c);

            if (discriminant < 0)
            {
                return true;
            }

            double rootOne = (-b + Math.Sqrt(discriminant)) / (2 * a);
            if (rootOne >= 0 && Math.Abs(rootOne - Math.Round(rootOne)) < 1e-9)
            {
                continue;
            }

            double rootTwo = (-b - Math.Sqrt(discriminant)) / (2 * a);
            if (rootTwo >= 0 && Math.Abs(rootTwo - Math.Round(rootTwo)) < 1e-9)
            {
                continue;
            }
            return true;
        }
        return false;
    }

    private bool SurelyWontLinearCollide1D(double p1, double v1, double p2, double v2)
    {
        // Particles get checked here if their accelerations are equal, then we just need to check if they could collide linearly.
        // That is where p1 + xv1 = p2 + xv2 has a non-negative integer root.
        // (p1 - p2) + (xv1 - xv2) = 0
        // (p2 - p1) / (v1 - v2) = x
        
        // We know their positions are not equal, because that gets checked at each tick simulation, so if velocities are equal, then they wont collide.
        if (v1 - v2 == 0)
        {
            return true;
        }

        double root = (p2 - p1) / (v1 - v2);
        if (root < 0 || Math.Abs(root - Math.Round(root)) >= 1e-9)
        {
            return true;
        }
        return false;
    }
}
