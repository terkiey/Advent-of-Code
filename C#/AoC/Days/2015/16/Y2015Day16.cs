using System.Text.RegularExpressions;

namespace AoC.Days;

internal class Y2015Day16 : Day
{
    private readonly static Regex sueCompoundPattern = new Regex(@"([a-z]+): (\d+)");
    private readonly Dictionary<string, int> MfcsamOutputDict = new()
    {
        { "children", 3 },
        { "cats", 7 },
        { "samoyeds", 2 },
        { "pomeranians", 3 },
        { "akitas", 0 },
        { "vizslas", 0 },
        { "goldfish", 5 },
        { "trees", 3 },
        { "cars", 2 },
        { "perfumes", 1 }
    };
    private readonly string[] Compounds = ["children", "cats", "samoyeds", "pomeranians", "akitas", "vizslas", "goldfish", "trees", "cars", "perfumes"];
    private readonly int[] MfcsamOutput = [3, 7, 2, 3, 0, 0, 5, 3, 2, 1];

    protected override void RunLogic(string[] inputLines)
    {
        for (int sueIndex = 0; sueIndex < inputLines.Count(); sueIndex++)
        {
            int[] parsedSue = ParseSue(inputLines[sueIndex]);
            if (ValidSue(parsedSue))
            {
                AnswerOne = (sueIndex + 1).ToString();
            }
            if (ValidSuePartTwo(parsedSue))
            {
                AnswerTwo = (sueIndex + 1).ToString();
            }
        }
    }

    private int[] ParseSue(string sueLine)
    {
        int[] parsedSue = new int[Compounds.Length];
        for (int compoundIndex = 0; compoundIndex < Compounds.Length; compoundIndex++)
        {
            parsedSue[compoundIndex] = -1;
        }
        foreach (Match match in sueCompoundPattern.Matches(sueLine))
        {
            string compound = match.Groups[1].Value;
            int quantity = int.Parse(match.Groups[2].Value);

            for (int compoundIndex = 0; compoundIndex < Compounds.Length; compoundIndex++)
            {
                if (compound == Compounds[compoundIndex])
                {
                    parsedSue[compoundIndex] = quantity;
                    break;
                }
            }
        }
        return parsedSue;
    }

    private bool ValidSue(int[] parsedSue)
    {
        for (int compoundIndex = 0; compoundIndex < Compounds.Length; compoundIndex++)
        {
            if (parsedSue[compoundIndex] == -1)
            {
                continue;
            }
            if (parsedSue[compoundIndex] != MfcsamOutput[compoundIndex])
            {
                return false;
            }
        }
        return true;
    }

    private bool ValidSuePartTwo(int[] parsedSue)
    {
        for (int compoundIndex = 0; compoundIndex < Compounds.Length; compoundIndex++)
        {
            if (parsedSue[compoundIndex] == -1)
            {
                continue;
            }
            if (compoundIndex == 1 || compoundIndex == 7)
            {
                if (parsedSue[compoundIndex] <= MfcsamOutput[compoundIndex])
                {
                    return false;
                }
            }
            else if (compoundIndex == 3 || compoundIndex == 5)
            {
                if (parsedSue[compoundIndex] >= MfcsamOutput[compoundIndex])
                {
                    return false;
                }
            }
            else if (parsedSue[compoundIndex] != MfcsamOutput[compoundIndex])
            {
                return false;
            }
        }
        return true;
    }
}
