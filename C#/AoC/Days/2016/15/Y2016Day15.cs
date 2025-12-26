namespace AoC.Days;

internal class Y2016Day15 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        DiscMazeMachine machine = new(inputLines);
        AnswerOne = machine.EarliestPassingDrop().ToString();
        machine.AddDisc(0, 11);
        AnswerTwo = machine.EarliestPassingDrop().ToString();
    }
}
