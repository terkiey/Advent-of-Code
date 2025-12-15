namespace AoC.Days;

internal class Y2025Day1 : Day
{
    protected override void RunLogic(string[] instructions)
    {
        ISafe safe = new Safe(50);
        ISafeTracker safeTracker = new SafeTracker(safe);

        List<int> dialTurns = safe.ReadInstructions(instructions);
        foreach (int dialTurn in dialTurns)
        {
            safe.TurnDial(dialTurn);
        }

        AnswerOne = safeTracker.Password.ToString();
        AnswerTwo = safeTracker.x434C49434B.ToString();
    }
}
