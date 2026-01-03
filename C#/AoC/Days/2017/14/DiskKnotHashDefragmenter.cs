using System.Drawing;

namespace AoC.Days;

internal class DiskKnotHashDefragmenter
{
    private readonly KnotHasher _knotHasher = new KnotHasher();
    private readonly KnotTyingProcessor _knotTyer = new KnotTyingProcessor();

    private bool[][] UsedGrid = new bool[128][];
    private bool[][] RegionGrid = new bool[128][];

    // For region counting
    private readonly List<Point> moves =
    [
        new(1, 0),
        new(-1, 0),
        new(0, 1),
        new(0, -1)
    ];

    private HashSet<Point> Visited = [];
    public int Regions = 0;

    public void BuildGrid(string seed)
    {
        for (int rowNum = 0; rowNum < 128; rowNum++)
        {
            string knotHashMe = seed + "-" + rowNum;
            _knotTyer.ProcessInstructions(knotHashMe, 256, true, 64);
            string knotHash = _knotHasher.GetKnotHash(_knotTyer.Numbers);
            UsedGrid[rowNum] = BuildRow(knotHash);
            RegionGrid[rowNum] = new bool[128];
        }
    }

    public int CountUsedSquares()
    {
        int usedCount = 0;
        for (int rowNum = 0; rowNum < UsedGrid.Length; rowNum++)
        {
            bool[] row = UsedGrid[rowNum];
            for (int colNum = 0; colNum < row.Length; colNum++)
            {
                usedCount += row[colNum] == true ? 1 : 0;
            }
        }
        return usedCount;
    }

    // Go through each row in the grid, whenever you hit a free tile not already assigned to a region increment regions by 1.
    // Then you do an exhaustive pathfinding to all possible squares and assigning their region numbers the same.
    // Then continue with the iterating through the rows of the grid.
    // Once you are done going through the grid, you are done.
    public void CalculateRegions()
    {
        Visited.Clear();
        for (int rowNum = 0; rowNum < RegionGrid.Length; rowNum++)
        {
            bool[] regionGridRow = RegionGrid[rowNum];
            bool[] usedGridRow = UsedGrid[rowNum];
            for (int colNum = 0; colNum < regionGridRow.Length; colNum++)
            {
                if (!regionGridRow[colNum] && usedGridRow[colNum])
                {
                    Regions++;
                    FillRegionIn(rowNum, colNum);
                }
            }
        }
    }

    private void FillRegionIn(int rowNum, int colNum)
    {
        Point start = new(rowNum, colNum);
        RegionGrid[rowNum][colNum] = true;
        foreach (Point neighbour in GetNeighbours(start))
        {
            FillRegionIn(neighbour.X, neighbour.Y);
        }
    }

    private IEnumerable<Point> GetNeighbours(Point start)
    {
        foreach (Point move in moves)
        {
            Point neighbour = new(start.X, start.Y);
            neighbour.Offset(move);
            if (neighbour.X < 0 || neighbour.X >= 128 || neighbour.Y < 0 || neighbour.Y >= 128)
            {
                continue;
            }

            if (Visited.Contains(neighbour))
            {
                continue;
            }

            if (!UsedGrid[neighbour.X][neighbour.Y] || RegionGrid[neighbour.X][neighbour.Y])
            {
                continue;
            }

            yield return neighbour;
        }
    }

    private bool[] BuildRow(string knotHash)
    {
        bool[] row = new bool[128];
        int byteCount = knotHash.Length / 2;
        byte[] bytes = Convert.FromHexString(knotHash);
        int index = 0;
        for (int byteIndex = 0; byteIndex < byteCount; byteIndex++)
        {
            for (int bit = 7; bit >= 0; bit--)
            {
                row[index++] = (bytes[byteIndex] >> bit & 1) == 1;
            }
        }
        return row;
    }

}
