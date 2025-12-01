using System.Reflection.Metadata.Ecma335;

namespace Day1;


internal class SafeTracker : ISafeTracker
{
    private readonly ISafe safe;

    public int Password { get; private set; }
    public int x434C49434B { get; private set; }

    public SafeTracker(ISafe inSafe)
    {
        safe = inSafe;

        safe.DialStopped += DialStoppedHandler;
        safe.DialClicked += DialClickedHandler;
    }

    private void DialStoppedHandler(object? sender, int dialValue)
    {
        if (dialValue == 0)
        {
            Password++;
        }
    }

    private void DialClickedHandler(object? sender, int clickValue)
    {
        if (clickValue % 100 == 0)
        {
            x434C49434B++;
        }
    }
}
