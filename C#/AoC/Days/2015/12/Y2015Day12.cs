using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace AoC.Days;

internal partial class Y2015Day12 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        string json = inputLines[0];
        json = json.Replace(" ", "");
        int numberSum = MatchNumbers().Matches(json)
            .Aggregate(0, (acc, match) =>
            {
                return acc += int.Parse(match.Value);
            });
        AnswerOne = numberSum.ToString();

        // To fix this, actually do JSON parsing. Regex wont work for part two.
        var RedObjects = MatchRedProperties().Matches(json)
            .Select(match => ParseRedObject(json, match.Index));
        var test = RedObjects.ToList();
        int RedObjectSum = RedObjects
                .Aggregate(0, (acc1, match1) =>
                {
                    return acc1 += MatchNumbers()
                        .Matches(match1)
                        .Aggregate(0, (acc2, match2) =>
                        {
                            return acc2 += int.Parse(match2.Value);
                        });
                });
        AnswerTwo = (numberSum - RedObjectSum).ToString();
    }

    [GeneratedRegex(@"(-?\d+)")]
    private static partial Regex MatchNumbers();

    [GeneratedRegex(@"(""[a-zA-Z]"":""red"")")]
    private static partial Regex MatchRedProperties();

    private string ParseRedObject(string json, int redStringIndex)
    {
        int lastOpeningBrace = -1;
        int cursorIndex = redStringIndex;
        while (lastOpeningBrace == -1)
        {
            cursorIndex--;
            if (json[cursorIndex] == '{')
            { lastOpeningBrace = cursorIndex; }
        }

        cursorIndex = redStringIndex;
        int braceLevel = 1;
        while (braceLevel > 0)
        {
            cursorIndex++;
            if (json[cursorIndex] == '}')
            { braceLevel--; }
            else if (json[cursorIndex] == '{')
            { braceLevel++; }
        }

        var test = json.Substring(lastOpeningBrace, cursorIndex - lastOpeningBrace + 1);
        return json.Substring(lastOpeningBrace, cursorIndex - lastOpeningBrace + 1);
    }
}
