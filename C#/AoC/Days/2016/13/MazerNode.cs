namespace AoC.Days;

internal class MazerNode
{
    public int[] LastMove { get; set; }
    public Position2D Position { get; set; }

    public MazerNode(int[] lastMove, int x, int y)
    {
        LastMove = lastMove;
        Position = new Position2D(x, y);
    }

    public MazerNode Move(int x, int y)
    {
        return new([x, y], Position.x + x, Position.y + y);
    }
}
