namespace AoC.Days;

internal class Y2025Day02 : Day
{
    protected override void RunLogic(string[] input)
    {
        IIdParser _idParser = new IdParser();
        List<string[]> ranges = _idParser.ParseIdRanges(input[0]);

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
