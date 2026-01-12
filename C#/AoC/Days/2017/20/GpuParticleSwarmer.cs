namespace AoC.Days;

internal class GpuParticleSwarmer
{
    private List<GpuParticle> Particles { get; } = [];

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
         * p + (v + (v+a) + (v+2a) + (v+3a)+ ... (v+xa)) =
         * p + xv + (a + 2a + ... + xa) =
         * p + xv + a(1 + 2 + 3 + ... + x) =
         * p + xv + a((x+1)x/2)
         * 
         * So we can solve for two different particles, if these formulas are equal to eachother...
         * p1 - p2 + xv1 - xv2 + a1((x+1)x/2) - a2((x+1)x/2) = 0
         * This can be solved with the quadratic formula (as its a quadratic equation), and if any positive roots exist that means a collision will occur, atleast for the position at time x on this axis.
         * 
         * So we invert it: if for any axis, there are no postive roots, then the particles will surely never collide.
         */
    }
}
