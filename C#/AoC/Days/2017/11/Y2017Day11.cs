namespace AoC.Days;

internal class Y2017Day11 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        HexGridPather pather = new();
        pather.TakePath(inputLines[0]);
        AnswerOne = pather.StepsFromOrigin().ToString();
        AnswerTwo = pather.FurthestFromOrigin.ToString();
    }
}
