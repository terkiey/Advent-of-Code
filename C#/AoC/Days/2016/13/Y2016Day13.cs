namespace AoC.Days;

internal class Y2016Day13 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        int xDest = -1;
        int yDest = -1;
        if (RunParameter == 1)
        {
            xDest = 31;
            yDest = 39;
        }
        else if (RunParameter == 2)
        {
            xDest = 7;
            yDest = 4;
        }

        BunnyHQMazer mazer = new(seed: inputLines[0]);
        AnswerOne = mazer.StepsRequiredFor(xDest, yDest).ToString();
        mazer = new(seed: inputLines[0]);
        AnswerTwo = mazer.DistinctTilesHit(steps: 50).ToString();
    }
}
