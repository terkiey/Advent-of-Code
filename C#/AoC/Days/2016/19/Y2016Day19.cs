using System.Text;

namespace AoC.Days;

internal class Y2016Day19 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        ElfElephantPartySimulator partySim = new();
        AnswerOne = partySim.LastElfLinear(inputLines[0]).ToString();
        AnswerTwo = partySim.LastElfAcross(inputLines[0]).ToString();
    }
}
