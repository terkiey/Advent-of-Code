namespace AoC.Days;

internal class Y2018Day06 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        ChronalCoordinateManager manager = new();
        manager.ParseData(inputLines);
        AnswerOne = manager.LargestNonInfiniteArea().ToString();
        AnswerTwo = manager.DistanceRegion().ToString();
    }
}
