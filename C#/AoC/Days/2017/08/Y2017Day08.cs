namespace AoC.Days;

internal class Y2017Day08 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        InsideTheCpu computer = new();
        computer.ProcessInstructions(inputLines);
        AnswerOne = computer.MaxRegValue().ToString();
        AnswerTwo = computer.highestRegisterEver.ToString();
    }
}
