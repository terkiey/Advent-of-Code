namespace AoC.Days;

internal class Y2018Day04 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        GuardSleepingPatternTracker tracker = new();
        tracker.ParseData(inputLines);
        AnswerOne = tracker.AnswerOne().ToString();
        AnswerTwo = tracker.AnswerTwo().ToString();
    }
}
