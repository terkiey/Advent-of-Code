namespace AoC.Days;

internal class Y2017Day22 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        SporificaVirusSimulator simulator = new();
        simulator.InitialiseState(inputLines);
        simulator.RunXBursts(10000);
        AnswerOne = simulator.InfectingBurstCount.ToString();
        simulator.InitialiseState(inputLines);
        simulator.RunXEvolvedBursts(10000000);
        AnswerTwo = simulator.InfectingBurstCount.ToString();
    }
}
