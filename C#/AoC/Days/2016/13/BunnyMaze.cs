namespace AoC.Days;

internal class BunnyMaze
{
    private readonly int mazeSeed;

    public bool[][] Maze { get; private set; }

    public BunnyMaze(int seed)
    {
        mazeSeed = seed;

        Maze = new bool[1][];
        Maze[0] = new bool[1];
        Maze[0][0] = GenerateTile(0, 0);
    }

    public void ExtendTo(int xMax, int yMax)
    {
        int xCurrent = Maze.Length;
        int yCurrent = Maze[0].Length;

        xMax = Math.Max(xMax + 1, xCurrent);
        yMax = Math.Max(yMax + 1, yCurrent);

        if (xCurrent == xMax && yCurrent == yMax)
        {
            return;
        }

        bool[][] newMaze = new bool[xMax][];
        for (int colIndex = 0; colIndex < xMax; colIndex++)
        {
            newMaze[colIndex] = new bool[yMax];
            if (colIndex < xCurrent)
            {
                ExtendColumn(newMaze[colIndex], Maze[colIndex], colIndex);
                Maze[colIndex].CopyTo(newMaze[colIndex], 0);
            }
            else
            {
                GenerateColumn(newMaze[colIndex], colIndex);
            }
        }
        Maze = newMaze;
    }

    private void GenerateColumn(bool[] newCol, int colIndex)
    {
        for (int y = 0; y < newCol.Length; y++)
        {
            newCol[y] = GenerateTile(colIndex, y);
        }
    }

    private void ExtendColumn(bool[] newCol, bool[] origCol, int colIndex)
    {
        origCol.CopyTo(newCol, 0);
        for (int y = origCol.Length; y < newCol.Length; y++)
        {
            newCol[y] = GenerateTile(colIndex, y);
        }
    }

    private bool GenerateTile(int x, int y)
    {
        int tileNum = (x * x) + (3 * x) + (2 * x * y) + y + (y * y) + mazeSeed;
        int oneBitCounter = 0;
        while (tileNum != 0)
        {
            oneBitCounter += (tileNum & 1) == 1 ? 1 : 0;
            tileNum >>= 1;
        }
        return oneBitCounter % 2 != 0;
    }
}
