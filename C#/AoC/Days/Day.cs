using System.Diagnostics;

namespace AoC.Days;
internal abstract class Day : IDay
{
    public string AnswerOne { get; protected set; } = String.Empty;
    public string AnswerTwo { get; protected set; } = String.Empty;
    public Stopwatch FileTimer { get; protected set; } = new Stopwatch();
    public Stopwatch LogicTimer { get; protected set; } = new Stopwatch();

    public int RunParameter;

    public void Main(DayArgs args)
    {
        string[] input = FileInput(args.filename, args.year);
        RunLogicTimed(input, args.runParameter);
    }

    protected virtual string[] FileInput(string filename, int year)
    {
        FileTimer.Start();
        string path = Path.Combine(AppContext.BaseDirectory, "Data", year.ToString(), filename);
        string[] input = File.ReadAllLines(path);
        FileTimer.Stop();

        return input;
    }

    protected void RunLogicTimed(string[] input, int runParameter)
    {
        LogicTimer.Start();
        AssignRunParameter(runParameter);
        RunLogic(input);
        LogicTimer.Stop();
    }

    protected void AssignRunParameter(int runParameter)
    {
        RunParameter = runParameter;
    }

    protected abstract void RunLogic(string[] input);
}
