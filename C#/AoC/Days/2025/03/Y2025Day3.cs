namespace AoC.Days;

internal class Y2025Day3 : Day
{
	protected override void RunLogic(string[] batteryBanks)
	{
        IBatteryChooser _batteryChooser = new BatteryChooser();

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
