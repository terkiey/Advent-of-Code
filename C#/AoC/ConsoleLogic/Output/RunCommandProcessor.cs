using AoC.Days;

namespace AoC.ConsoleLogic;

internal class RunCommandProcessor : IRunCommandProcessor
{
    public Dictionary<int, Func<IDay>> dayFactories = new()
    {
        { 1, () => new Day1() },
        { 2, () => new Day2() },
        { 3, () => new Day3() },
        { 4, () => new Day4() },
        { 5, () => new Day5() },
        { 6, () => new Day6() },
    };

    public RunCommandProcessor() { }

    public void Process(RunCommand command)
    {
        int day = command.Day;
        FileType fileType = command.FileType;
        string filename = GetFilename(day, fileType);

        switch (command.RunType)
        {
            case RunType.Solve:
                ProcessSolve(day, filename);
                break;

            case RunType.Test:
                ProcessTest(day, filename, command.TestRuns!.Value);
                break;

            default:
                throw new Exception("WHAT THE FUCK");
        }
    }

    private void ProcessSolve(int day, string filename)
    {
        if (!dayFactories.TryGetValue(day, out Func<IDay>? dayFactory))
        {
            Console.WriteLine($"Factory not found for day {day}.");
            return;
        }

        IDay dayProgram = dayFactory();
        DayArgs dayArgs = new(filename);
        dayProgram.Main(dayArgs);

        Console.WriteLine($"Part one answer: {dayProgram.AnswerOne}");
        Console.WriteLine($"Part two answer: {dayProgram.AnswerTwo}");
    }

    private void ProcessTest(int day, string filename, int testRuns)
    {
        if (!dayFactories.TryGetValue(day, out Func<IDay>? dayFactory))
        {
            Console.WriteLine($"Factory not found for day {day}.");
            return;
        }

        IDay dayProgram = dayFactory();
        DayArgs dayArgs = new(filename);

        List<long> fileTimes = [];
        List<long> logicTimes = [];
        for (int testIndex = 0; testIndex < testRuns; testIndex++)
        {
            dayProgram.Main(dayArgs);
            fileTimes.Add(dayProgram.FileTimer.ElapsedMilliseconds);
            logicTimes.Add(dayProgram.LogicTimer.ElapsedMilliseconds);
        }

        string col1;
        string col2;
        string col3;
        string col4;
        string colSep = " | ";

        col1 = $"Day {day}";
        col2 = $"Tests: {testRuns}";
        Console.WriteLine(col1.PadRight(10) + colSep + col2.PadRight(15));


        col1 = "Filetimes";
        col2 = $"Max: {fileTimes.Max()}ms";
        col3 = $"Min: {fileTimes.Min()}ms";
        col4 = $"Average: {Math.Ceiling(fileTimes.Average())}ms";
        Console.WriteLine(col1.PadRight(10) + colSep + col2.PadRight(15) + colSep + col3.PadRight(15) + colSep + col4.PadRight(20));

        col1 = "Logictimes";
        col2 = $"Max: {logicTimes.Max()}ms";
        col3 = $"Min: {logicTimes.Min()}ms";
        col4 = $"Average: {Math.Ceiling(logicTimes.Average())}ms";
        Console.WriteLine(col1.PadRight(10) + colSep + col2.PadRight(15) + colSep + col3.PadRight(15) + colSep + col4.PadRight(20));
    }

    // TODO_MID: Write a reusable command that takes a list of strings, "columnList" and places them padded by a given padList, with a given separator character between.
    private string GetFilename(int day, FileType fileType)
    {
        switch (fileType)
        {
            case FileType.Real:
                return "day" + day + "input.txt";

            case FileType.Sample:
                return "day" + day + "testinput.txt";

            default:
                throw new Exception("WHAT THE FUCK");
        }
    }
}
