namespace AoC.Days;

internal class Y2017Day20 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        GpuParticleSwarmer swarmer = new();
        swarmer.ReadParticleData(inputLines);
        AnswerOne = swarmer.GetClosestLongTermToOrigin();
        swarmer.ReadParticleData(inputLines);
        AnswerTwo = swarmer.GetCountInfiniteParticles();
    }
}
