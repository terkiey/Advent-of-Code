namespace AoC.Days;

internal class Y2016Day22 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        NodeGridComputer computer = new();
        computer.DeduceState(inputLines);
        AnswerOne = computer.ViablePairs().ToString();
        SimplifiedNodeGrid nodeGridModel = new();
        nodeGridModel.DeduceSimplifiedModel(computer._network);
        AnswerTwo = nodeGridModel.LeastStepsForTransfer().ToString();
    }
}
