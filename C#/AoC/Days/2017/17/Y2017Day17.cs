namespace AoC.Days;

internal class Y2017Day17 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        SpinLockSimulator simulator = new();
        simulator.SimulatePartOne(inputLines[0]);
        AnswerOne = simulator.PartOneValue.ToString();
        simulator.SimulatePartTwo(inputLines[0]);
        AnswerTwo = simulator.PartTwoValue.ToString();
    }
}
