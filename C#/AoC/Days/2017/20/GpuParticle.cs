namespace AoC.Days;

internal class GpuParticle
{
    public int[] Position { get; }
    public int[] Velocity { get; }
    public int[] Acceleration { get; }
    public int Id { get; }


    public GpuParticle(int[] position, int[] velocity, int[] acceleration, int id)
    {
        Position = position;
        Velocity = velocity;
        Acceleration = acceleration;
        Id = id;
    }
}
