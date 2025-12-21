namespace AoC.Days;

internal class KeypadTraverser
{
    private readonly char[,] keypad = new char[3, 3]
    {
        {'1', '2', '3' },
        {'4', '5', '6' },
        {'7', '8', '9' }
    };
    private int xPos;
    private int yPos;
    private readonly char[][] fuckedKeypad =
    [
        ['?','?','1','?','?'],
        ['?','2','3','4','?'],
        ['5','6','7','8','9'],
        ['?','A','B','C','?'],
        ['?','?','D','?','?']
    ];


    public string GetBathroomCode(string[] inputLines)
    {
        xPos = 1;
        yPos = 1;
        char[] code = new char[inputLines.Length];
        int codeIndex = 0;
        foreach(string line in inputLines)
        {
            code[codeIndex] = TraverseKeypad(line);
            codeIndex++;
        }
        return new(code);
    }

    public char TraverseKeypad(string moves)
    {
        foreach(char move in moves)
        {
            switch(move)
            {
                case 'R':
                    xPos++;
                    break;

                case 'L':
                    xPos--;
                    break;

                case 'U':
                    yPos--;
                    break;

                case 'D':
                    yPos++;
                    break;
            }

            xPos = Math.Max(0, xPos);
            xPos = Math.Min(2, xPos);
            yPos = Math.Max(0, yPos);
            yPos = Math.Min(2, yPos);
        }
        return keypad[yPos, xPos];
    }

    public string GetFuckedBathroomCode(string[] inputLines)
    {
        xPos = 0;
        yPos = 2;
        char[] code = new char[inputLines.Length];
        int codeIndex = 0;
        foreach (string line in inputLines)
        {
            code[codeIndex] = TraverseFuckedKeypad(line);
            codeIndex++;
        }
        return new(code);
    }

    public char TraverseFuckedKeypad(string moves)
    {
        foreach(char move in moves)
        {
            switch (move)
            {
                case 'R':
                    xPos++;
                    xPos = ValidateFuckedX(xPos, yPos);
                    break;

                case 'L':
                    xPos--;
                    xPos = ValidateFuckedX(xPos, yPos);
                    break;

                case 'U':
                    yPos--;
                    yPos = ValidateFuckedY(xPos, yPos);
                    break;

                case 'D':
                    yPos++;
                    yPos = ValidateFuckedY(xPos, yPos);
                    break;
            }
        }
        return fuckedKeypad[yPos][xPos];
    }

    private int ValidateFuckedX(int xPos, int yPos)
    {
        if (yPos == 0 || yPos == 4)
        {
            xPos = 2;
        }
        else if (yPos == 1 || yPos == 3)
        {
            xPos = Math.Max(1, xPos);
            xPos = Math.Min(3, xPos);
        }
        else if (yPos == 2)
        {
            xPos = Math.Max(0, xPos);
            xPos = Math.Min(4, xPos);
        }
        return xPos;
    }

    private int ValidateFuckedY(int xPos, int yPos)
    {
        if (xPos == 0 || xPos == 4)
        {
            yPos = 2;
        }
        else if (xPos == 1 || xPos == 3)
        {
            yPos = Math.Max(1, yPos);
            yPos = Math.Min(3, yPos);
        }
        else if (xPos == 2)
        {
            yPos = Math.Max(0, yPos);
            yPos = Math.Min(4, yPos);
        }
        return yPos;
    }
}
