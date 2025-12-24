namespace AoC.Days;

internal class Y2016Day14 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        OneTimePadCalculator calculator = new();
        calculator.Key64Made += (_, index) =>
        {
            if (AnswerOne == String.Empty)
            {
                AnswerOne = index.ToString();
            }
            else
            {
                AnswerTwo = index.ToString();
            }
        };
        calculator.ConfigureSalt(inputLines[0]);
        calculator.MakeKeysForPartOne();
        calculator.ClearResults();
        calculator.MakeStretchedKeysForPartTwo();
    }
}
