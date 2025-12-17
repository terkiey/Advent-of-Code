namespace AoC.Days;

internal class AnimatedLightBoard
{
    private const int gridWidth = 100;
    private const int gridHeight = 100;
    private readonly bool[,] _lightGrid; 

    public AnimatedLightBoard(string[] inputLines)
    {
        _lightGrid = InitialiseLightGrid(inputLines);    
    }

    public void Step(int stepCount)
    {
        for (int step = 0; step < stepCount; step++)
        {
            bool[,] initialGridState = (bool[,])_lightGrid.Clone();
            int[,] surroundingOnLights = CountSurroundingOnLights(initialGridState);
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    _lightGrid[x, y] = CalculateLightState(surroundingOnLights[x, y], initialGridState[x, y]);
                }
            }
        }
    }

    public void StepPartTwo(int stepCount)
    {
        foreach (var (x, y) in Corners)
        {
            _lightGrid[x, y] = true;
        }
        for (int step = 0; step < stepCount; step++)
        {
            bool[,] initialGridState = (bool[,])_lightGrid.Clone();
            int[,] surroundingOnLights = CountSurroundingOnLights(initialGridState);
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    if (Corners.Contains((x,y)))
                    {
                        continue;
                    }
                    _lightGrid[x, y] = CalculateLightState(surroundingOnLights[x, y], initialGridState[x, y]);
                }
            }
        }
    }

    public int CountLightsOn()
    {
        int count = 0;
        foreach (bool light in _lightGrid)
        {
            if (light)
            { count++; }
        }
        return count;
    }

    private bool[,] InitialiseLightGrid(string[] inputLines)
    {
        bool[,] lightGrid = new bool[100, 100];
        for (int y = 0; y < gridHeight; y++)
        {
            string rowString = inputLines[y];
            for (int x = 0; x < gridWidth; x++)
            {
                lightGrid[x, y] = rowString[x] == '#' ? true : false;
            }
        }
        return lightGrid;
    }

    private bool CalculateLightState(int surroundingOnLights, bool initialState)
    {
        switch (initialState)
        {
            case true:
                if (surroundingOnLights == 2 || surroundingOnLights == 3)
                {
                    return true;
                }
                return false;

            case false:
                if (surroundingOnLights == 3)
                {
                    return true;
                }
                return false;
        }
    }

    private int[,] CountSurroundingOnLights(bool[,] gridState)
    {
        int[,] surroundingLightsOnGrid = new int[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                foreach ((int dx, int dy) in Neighbours)
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx < 0 || ny < 0 || nx > gridWidth - 1 || ny > gridHeight - 1)
                    {
                        continue;
                    }
                    if (gridState[nx, ny] == true) 
                    {
                        surroundingLightsOnGrid[x, y]++;
                    }
                }
            }
        }
        return surroundingLightsOnGrid;
    }

    private readonly static (int dx, int dy)[] Neighbours =
    [
        (-1, -1), (0, -1), (1, -1),
        (-1,  0),          (1,  0),
        (-1,  1), (0,  1), (1,  1)
    ];

    private readonly static (int x, int y)[] Corners =
    [
        (0, 0),              (gridWidth - 1, 0),
        (0, gridHeight - 1), (gridWidth - 1, gridHeight - 1)
    ];
}
