namespace AoC;

/// <summary>
/// This is the composition root.
/// </summary>
internal class Day1 : IDay
{
    public string AnswerOne { get; private set; } = String.Empty;
    public string AnswerTwo { get; private set; } = String.Empty;

    public void Main()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Data", "day1input.txt");
        string[] input = File.ReadAllLines(path);
        ISafe safe = new Safe(50);
        ISafeTracker safeTracker = new SafeTracker(safe);

        List<int> dialTurns = safe.ReadInstructions(input);
        
        foreach(int dialTurn in dialTurns)
        {
            safe.TurnDial(dialTurn);
        }

        AnswerOne = safeTracker.Password.ToString();
        AnswerTwo = safeTracker.x434C49434B.ToString();
    }
}
