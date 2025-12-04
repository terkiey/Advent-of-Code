namespace AoC;

internal class Day3 : IDay
{
    public string AnswerOne { get; private set; } = String.Empty;
    public string AnswerTwo { get; private set; } = String.Empty;

    public void Main()
    {
        IBatteryChooser _batteryChooser = new BatteryChooser();

        string path = Path.Combine(AppContext.BaseDirectory, "Data", "day3input.txt");
        string[] batteryBanks = File.ReadAllLines(path);

        long sumJoltage = 0;
        foreach(string batteryBank in batteryBanks)
        {
            sumJoltage += _batteryChooser.Choose2Batteries(batteryBank);
        }
        AnswerOne = sumJoltage.ToString();
        sumJoltage = 0;

        foreach (string batteryBank in batteryBanks)
        {
            sumJoltage += _batteryChooser.Choose12Batteries(batteryBank);
        }

        AnswerTwo = sumJoltage.ToString();
    }
}
