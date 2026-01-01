using System.Text.RegularExpressions;

namespace AoC.Days;

internal class Y2017Day02 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        int checksum = 0;
        foreach (string row in inputLines)
        {
            MatchCollection numberMatches = Regex.Matches(row, @"(\d+)");
            int min = int.MaxValue;
            int max = int.MinValue;
            foreach (Match numberMatch in numberMatches)
            {
                min = int.Parse(numberMatch.Value) < min ? int.Parse(numberMatch.Value) : min;
                max = int.Parse(numberMatch.Value) > max ? int.Parse(numberMatch.Value) : max;
            }
            checksum += max - min;
        }
        AnswerOne = checksum.ToString();

        checksum = 0;
        foreach (string row in inputLines)
        {
            MatchCollection numberMatches = Regex.Matches(row, @"(\d+)");
            List<int> numbers = [];
            foreach (Match numberMatch in numberMatches)
            {
                numbers.Add(int.Parse(numberMatch.Value));
            }

            for (int numberIndex = 0; numberIndex < numbers.Count - 1; numberIndex++)
            {
                int number = numbers[numberIndex];
                for (int otherIndex = numberIndex + 1; otherIndex < numbers.Count - 1; otherIndex++)
                {
                    int otherNumber = numbers[otherIndex];
                    if (otherNumber % number == 0)
                    {
                        checksum += otherNumber / number;
                    }
                    else if (number % otherNumber == 0)
                    {
                        checksum += number / otherNumber;
                    }
                }
            }
        }
        AnswerTwo = checksum.ToString();
    }
}
