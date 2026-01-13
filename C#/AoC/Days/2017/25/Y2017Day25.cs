namespace AoC.Days;

internal class Y2017Day25 : Day
{

    // My initial thought is that this is another peephole optimisation, though I will try a naive approach first.
    protected override void RunLogic(string[] inputLines)
    {
        TuringMachineInsideTheCpu machine = new();
        machine.ParseBlueprint(inputLines);
        machine.Run();
        AnswerOne = machine.CalculateCheckSum().ToString();
    }
}
