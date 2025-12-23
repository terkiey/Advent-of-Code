namespace AoC.Days;

internal class Y2016Day12 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        AssembunnyComputer computer = new();
        computer.ProcessInstructions(inputLines);
        AnswerOne = computer.a.ToString();
        computer.ClearRegisters();
        computer.c = 1;
        computer.ProcessInstructions(inputLines);
        AnswerTwo = computer.a.ToString();
    }
}
