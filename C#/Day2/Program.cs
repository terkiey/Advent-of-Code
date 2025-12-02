using System.Runtime.InteropServices;

namespace Day2;

internal class Program
{
    static async Task Main(string[] args)
    {
        string input = File.ReadAllLines("day2input.txt")[0];

        IIdParser _idParser = new IdParser();
        List<string[]> ranges = _idParser.ParseIdRanges(input);

        double PartOneSum = 0;
        double PartTwoSum = 0;
        IIdValidator _idValidator = new IdValidator();
        foreach (string[] range in ranges)
        {
            double rangeStart = double.Parse(range[0]);
            double rangeEnd = double.Parse(range[1]);

            for(double id = rangeStart; id <= rangeEnd; id++)
            {
                string idString = id.ToString();
                if (!_idValidator.ValidateIdPartOne(idString))
                {
                    PartOneSum += id;
                }

                if (!_idValidator.ValidateIdPartTwo(idString))
                {
                    PartTwoSum += id;
                }
            }
        }

        Console.WriteLine($"Part One Answer : {PartOneSum}");
        Console.WriteLine($"Part Two Answer : {PartTwoSum}");
        await Task.Delay(5000);
    }
}
