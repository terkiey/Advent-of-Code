namespace AoC.Days;

internal class TrapLayerBot
{
    private string Seed { get; set; } = string.Empty;
    public void SetSeed(string firstRow)
    {
        Seed = firstRow;
    }

    public int SafeTiles(int rowsToCheck)
    {
        char[] lastRow = Seed.ToCharArray();
        int safeCount = lastRow.Count(c => c == '.');
        int rowCounter = 1;

        while (rowCounter < rowsToCheck)
        {
            lastRow = GetNextRow(lastRow);
            safeCount += lastRow.Count(c => c == '.');
            rowCounter++;
        }
        return safeCount;
    }

    private char[] GetNextRow(char[] row)
    {
        char[] nextRow = new char[row.Length];
        for (int newTileIndex = 0; newTileIndex < row.Length;  newTileIndex++)
        {
            bool leftIsTrap = false;
            bool centreIsTrap = false;
            bool rightIsTrap = false;
            for (int indexOffset = -1; indexOffset <= 1;  indexOffset++)
            {
                bool trapFound = false;
                int indexOfCheck = newTileIndex + indexOffset;
                if (indexOfCheck >= 0 && indexOfCheck < row.Length)
                {
                    trapFound = row[indexOfCheck] == '^' ? true : false;
                }

                switch (indexOffset)
                {
                    case -1:
                        leftIsTrap = trapFound;
                        break;

                    case 0:
                        centreIsTrap = trapFound;
                        break;

                    case 1:
                        rightIsTrap = trapFound;
                        break;
                }
            }

            if (leftIsTrap != rightIsTrap)
            {
                nextRow[newTileIndex] = '^';
            }
            else
            {
                nextRow[newTileIndex] = '.';
            }
        }
        return nextRow;
    }
}
