using System.Drawing;

namespace AoC.Days;

internal class HexGridPather
{
    private Point GridPos = new(0, 0);
    private Dictionary<string, Point> Moves { get; } = new()
    {
        ["nw"] = new(-1, 1),
        ["n"] = new(0, 2),
        ["ne"] = new(1, 1),
        ["se"] = new(1, -1),
        ["s"] = new(0, -2),
        ["sw"] = new(-1, -1),
    };

    public int FurthestFromOrigin { get; set; } = 0;

    public void TakePath(string path)
    {
        string[] moves = path.Split(',');
        foreach (string move in moves)
        {
            Point movePoint = Moves[move];
            GridPos.Offset(movePoint);
            int distance = StepsFromOrigin();
            FurthestFromOrigin = FurthestFromOrigin < distance ? distance : FurthestFromOrigin;
        }
    }

    public int StepsFromOrigin()
    {
        Point position = new(Math.Abs(GridPos.X), Math.Abs(GridPos.Y));
        if (position.X > position.Y)
        {
            return position.X;
        }
        int diagonalSteps = Math.Min(position.X, position.Y);
        int nsSteps = (Math.Max(position.X, position.Y) - diagonalSteps) / 2;
        return diagonalSteps + nsSteps;
    }
}
