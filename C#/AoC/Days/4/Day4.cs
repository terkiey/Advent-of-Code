namespace AoC;

internal class Day4 : IDay
{
    public string AnswerOne { get; private set; } = String.Empty;
    public string AnswerTwo { get; private set; } = String.Empty;

    public void Main()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Data", "day4testinput.txt");
        List<string> gridRows = File.ReadAllLines(path).ToList();
    }
}
