namespace AoC.Days;

internal class Y2016Day23 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        AssembunnyComputerV2 computer = new();
        computer.a = 7;
        computer.ProcessInstructions(inputLines);
        AnswerOne = computer.a.ToString();

        computer.ClearRegisters();
        computer.a = 12;
        computer.ProcessInstructions(inputLines);
        AnswerTwo = computer.a.ToString();
    }
}
