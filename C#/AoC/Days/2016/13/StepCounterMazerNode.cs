namespace AoC.Days;
internal class StepCounterMazerNode
{
    public int[] LastMove { get; set; }
    public Position2D Position { get; set; }
    public int Steps { get; set; }

    public StepCounterMazerNode(int[] lastMove, int x, int y, int steps)
    {
        LastMove = lastMove;
        Position = new Position2D(x, y);
        Steps = steps;
    }

    public StepCounterMazerNode Move(int x, int y)
    {
        return new([x, y], Position.x + x, Position.y + y, Steps + 1);
    }
}
