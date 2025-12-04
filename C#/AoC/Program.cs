using System.Diagnostics;
using System.Globalization;

namespace AoC;

internal class Program
{
    public static Dictionary<int, Func<IDay>> dayFactories = new()
    {
        { 1, () => new Day1() },
        { 2, () => new Day2() },
        { 3, () => new Day3() },
        { 4, () => new Day4() },
    };

    static void Main(string[] args)
    {
        int day = 0;

        while (true)
        {
        Console.WriteLine("Type the day number as an integer (12, 3, 7, 20)");
        string? userInput = Console.ReadLine();
    
        bool daySet = false;
            while (!daySet)
            {
                if (userInput is null)
                {
                    Console.WriteLine("kys - nothing input");
                    continue;
                }

                if (userInput.ToLower() == "quit" || userInput.ToLower() == "exit" || userInput.ToLower() == "stop" || userInput.ToLower() == "cancel" || userInput.ToLower() == "q")
                {
                    Console.WriteLine("Ok, quitting.");
                    Environment.Exit(2);
                }

                try
                {
                    day = int.Parse(userInput);
                }
                catch
                {
                    Console.WriteLine("kys - wrong format");
                    continue;
                }

                if (day < 1 || day > 25)
                {
                    Console.WriteLine("kys - not an advent day");
                    continue;
                }

                daySet = true;
            }

            daySet = false;

            Console.WriteLine($"Retrieving program factory for selected day: Day{day}.");
            if (!dayFactories.TryGetValue(day, out Func<IDay>? dayFactory))
            {
                Console.WriteLine($"Factory not found for day {day}.");
                Console.WriteLine("Do the day's puzzle!");
                Environment.Exit(1);
            }

            Console.WriteLine($"Starting timer");
            Stopwatch stopwatch = Stopwatch.StartNew();

            IDay dayProgram = dayFactory();
            dayProgram.Main();

            stopwatch.Stop();

            Console.WriteLine($"Day{day} | Part one answer: {dayProgram.AnswerOne}");
            Console.WriteLine($"Day{day} | Part two answer: {dayProgram.AnswerTwo}");
            Console.WriteLine($"Day{day} | Time elapsed: {stopwatch.Elapsed.TotalMilliseconds}ms");
        }
    }
}
