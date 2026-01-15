namespace AoC.Days;

internal class Y2018Day05 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        AlchemicalReductionChemicalFacility reducer = new();
        string reducedPolymer = reducer.ReducePolymer(inputLines[0]);
        AnswerOne = reducedPolymer.Length.ToString();
        AnswerTwo = reducer.PowerReducePolymer(inputLines[0]).ToString();
    }
}
