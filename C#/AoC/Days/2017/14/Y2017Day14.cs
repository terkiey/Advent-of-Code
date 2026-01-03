namespace AoC.Days;

internal class Y2017Day14 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        DiskKnotHashDefragmenter defragger = new();
        defragger.BuildGrid(inputLines[0]);
        AnswerOne = defragger.CountUsedSquares().ToString();
        defragger.CalculateRegions();
        AnswerTwo = defragger.Regions.ToString();
    }
}
