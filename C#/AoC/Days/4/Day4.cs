namespace AoC;

internal class Day4 : IDay
{
    public string AnswerOne { get; private set; } = String.Empty;
    public string AnswerTwo { get; private set; } = String.Empty;

    public void Main()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Data", "day4input.txt");
        string[] gridRowStrings = File.ReadAllLines(path);

        IRollRater _paperRater = new RollRater(gridRowStrings);

        _paperRater.RateRolls();
        int peelCounter = _paperRater.AccessibleRollCount;
        AnswerOne = peelCounter.ToString();

        while (_paperRater.PeelLayer())
        {
            _paperRater.RateRolls();
            peelCounter += _paperRater.AccessibleRollCount;
        }

        AnswerTwo = peelCounter.ToString();
    }
}
