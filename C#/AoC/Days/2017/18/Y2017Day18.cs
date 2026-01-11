namespace AoC.Days;

internal class Y2017Day18 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        DuetComputer computer = new();
        computer.ParseInstructions(inputLines);
        computer.ConnectEvents();
        computer.RunSoloMode();
        AnswerOne = computer.PartOneAnswer;
        computer.RunDuetMode();
        AnswerTwo = computer.PartTwoAnswer;
    }
}
