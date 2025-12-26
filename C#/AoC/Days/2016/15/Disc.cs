namespace AoC.Days;

internal class Disc
{
    public int PositionCount { get; set; }
    public int Position { get; set; }

    public Disc(int startPosition, int positionCount)
    {
        Position = startPosition;
        PositionCount = positionCount;
    }
}
