using System.Runtime.Serialization;

namespace Day1;

/// <summary>
/// This is the composition root.
/// </summary>
internal class Program
{
    static async Task Main(string[] args)
    {
        string[] input = File.ReadAllLines("day1input.txt");
        ISafe safe = new Safe(50);
        ISafeTracker safeTracker = new SafeTracker(safe);

        List<int> dialTurns = safe.ReadInstructions(input);
        
        foreach(int dialTurn in dialTurns)
        {
            safe.TurnDial(dialTurn);
        }

        Console.WriteLine($"Part One Answer = {safeTracker.Password}");
        Console.WriteLine($"Part Two Answer = {safeTracker.x434C49434B}");

        await Task.Delay(5000);
    }
}
