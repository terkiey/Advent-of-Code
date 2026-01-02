namespace AoC.Days;

internal class Y2017Day12 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        PipeGraphManager manager = new();
        manager.CreateGraph(inputLines);
        AnswerOne = manager.ProgramsInGroupX[0].Count.ToString();
        AnswerTwo = manager.ProgramsInGroupX.Count.ToString();
    }
}
