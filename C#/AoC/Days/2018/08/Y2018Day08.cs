namespace AoC.Days;

internal class Y2018Day08 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        SleighNavigationMemoryTreeBuilder builder = new(inputLines[0]);
        builder.BuildTree();
        AnswerOne = builder.SumRootMetadata().ToString();
        AnswerTwo = builder.GetRootValue().ToString();
    }
}
