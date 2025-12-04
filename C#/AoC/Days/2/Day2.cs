namespace AoC;

/// <summary>
/// This is the composition root.
/// </summary>
internal class Day2 : IDay
{
    public string AnswerOne { get; private set; } = String.Empty;
    public string AnswerTwo { get; private set; } = String.Empty;

    public void Main()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Data", "day2input.txt");
        string input = File.ReadAllLines(path)[0];

        IIdParser _idParser = new IdParser();
        List<string[]> ranges = _idParser.ParseIdRanges(input);

        double PartOneSum = 0;
        double PartTwoSum = 0;
        IIdValidator _idValidator = new IdValidator();
        foreach (string[] range in ranges)
        {
            double rangeStart = double.Parse(range[0]);
            double rangeEnd = double.Parse(range[1]);

            for (double id = rangeStart; id <= rangeEnd; id++)
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

        AnswerOne = PartOneSum.ToString();
        AnswerTwo = PartTwoSum.ToString();
    }
}
