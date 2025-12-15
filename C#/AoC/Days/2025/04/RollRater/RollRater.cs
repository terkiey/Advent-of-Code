namespace AoC.Days;

internal class RollRater : IRollRater
{
    private readonly char[][] _grid;
    private readonly int _gridRowCount;
    private readonly int _gridColCount;


    private int[][] rateMap;
    private bool[][] accessibleRollGrid;

    public int AccessibleRollCount
    {
        get
        {
            return accessibleRollGrid.SelectMany(inner => inner).Count(gridIsAccessibleRoll => gridIsAccessibleRoll);
        }
    }

    public RollRater(string[] gridRowStrings)
    {
        _gridRowCount = gridRowStrings.Count();
        _gridColCount = gridRowStrings[0].Length;

        _grid = new char[_gridRowCount][];
        rateMap = new int[_gridRowCount][];
        accessibleRollGrid = new bool[_gridRowCount][];

        for (int rowIndex = 0; rowIndex < _gridRowCount; rowIndex++)
        {
            rateMap[rowIndex] = new int[_gridColCount];
            _grid[rowIndex] = gridRowStrings[rowIndex].ToCharArray();
            accessibleRollGrid[rowIndex] = new bool[_gridColCount];
        }
    }

    public void RateRolls()
    {
        rateMap = new int[_gridRowCount][];
        for (int rowIndex = 0; rowIndex < _gridRowCount; rowIndex++)
        {
            rateMap[rowIndex] = new int[_gridColCount];
        }

        for (int rowIndex = 0; rowIndex < _gridRowCount; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < _gridColCount; columnIndex++)
            {
                if (_grid[rowIndex][columnIndex] != '@') { continue; }

                AddToSurroundingRates(rowIndex, columnIndex);
            }
        }


        for (int rowIndex = 0; rowIndex < _gridRowCount; rowIndex++)
        {
            for (int colIndex = 0; colIndex < _gridColCount; colIndex++)
            {
                if (_grid[rowIndex][colIndex] == '@' && rateMap[rowIndex][colIndex] < 5)
                {
                    accessibleRollGrid[rowIndex][colIndex] = true;
                }
            }
        }
    }

    public bool PeelLayer()
    {
        bool anythingPeeled = false;
        for (int rowIndex = 0; rowIndex < _gridRowCount; rowIndex++)
        {
            for (int colIndex = 0; colIndex < _gridColCount; colIndex++)
            {
                if (!accessibleRollGrid[rowIndex][colIndex]) { continue; }
                _grid[rowIndex][colIndex] = '.';
                accessibleRollGrid[rowIndex][colIndex] = false;
                anythingPeeled = true;
            }
        }

        return anythingPeeled;
    }

    private void AddToSurroundingRates(int rowIndex, int colIndex)
    {
        int minRowShift = -1;
        int maxRowShift = 1;
        int minColShift = -1;
        int maxColShift = 1;

        if (rowIndex == 0) { minRowShift = 0; }
        if (rowIndex == _gridRowCount - 1) { maxRowShift = 0; }
        if (colIndex == 0) { minColShift = 0; }
        if (colIndex == _gridColCount - 1) { maxColShift = 0; }

        foreach (int rowShift in Enumerable.Range(minRowShift, maxRowShift - minRowShift + 1))
        {
            int shiftedRowIndex = rowIndex + rowShift;
            foreach (int colShift in Enumerable.Range(minColShift, maxColShift - minColShift + 1))
            {
                int shiftedColIndex = colIndex + colShift;
                if (_grid[shiftedRowIndex][shiftedColIndex] == '@')
                {
                    rateMap[shiftedRowIndex][shiftedColIndex]++;
                }
            }
        }
    }
}
