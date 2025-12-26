namespace AoC.Days;

internal class Y2016Day18 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        TrapLayerBot trapLayer = new();
        int rowsToCheck = -1;
        if (RunParameter == 1)
        {
            rowsToCheck = 40;
        }
        else if (RunParameter == 2)
        {
            rowsToCheck = 10;
        }

        trapLayer.SetSeed(inputLines[0]);
        AnswerOne = trapLayer.SafeTiles(rowsToCheck).ToString();
        AnswerTwo = trapLayer.SafeTiles(400000).ToString();
    }
}
