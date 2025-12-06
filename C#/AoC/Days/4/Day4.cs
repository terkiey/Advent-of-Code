namespace AoC.Days;

internal class Day4 : Day
{
    protected override void RunLogic(string[] gridRowStrings)
    {
        IRollRater _rollRater = new RollRater(gridRowStrings);

        _rollRater.RateRolls();
        int peelCounter = _rollRater.AccessibleRollCount;
        AnswerOne = peelCounter.ToString();

        while (_rollRater.PeelLayer())
        {
            _rollRater.RateRolls();
            peelCounter += _rollRater.AccessibleRollCount;
        }

        AnswerTwo = peelCounter.ToString();
    }
}
