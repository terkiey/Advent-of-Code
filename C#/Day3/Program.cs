namespace Day3;

internal class Program
{
    static void Main(string[] args)
    {
        IBatteryChooser _batteryChooser = new BatteryChooser();

        string[] batteryBanks = File.ReadAllLines("day3input.txt");
        long sumJoltage = 0;
        foreach(string batteryBank in batteryBanks)
        {
            sumJoltage += _batteryChooser.Choose2Batteries(batteryBank);
        }
        Console.WriteLine($"Part One Answer : " + sumJoltage);
        sumJoltage = 0;

        foreach (string batteryBank in batteryBanks)
        {
            sumJoltage += _batteryChooser.Choose12Batteries(batteryBank);
        }

        Console.WriteLine($"Part Two Answer : " + sumJoltage);
    }
}
