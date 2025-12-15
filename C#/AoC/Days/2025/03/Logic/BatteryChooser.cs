namespace AoC.Days;

internal class BatteryChooser : IBatteryChooser
{
    public int Choose2Batteries(string bank)
    {

        int firstDigitIndex = 0;
        int firstDigit = 0;
        int secondDigit = 0;

        for(int digitIndex = 0; digitIndex < bank.Length - 1;  digitIndex++)
        {
            string digitString = bank[digitIndex].ToString();
            int digit = int.Parse(digitString);
            if (digit > firstDigit)
            {
                firstDigit = digit;
                firstDigitIndex = digitIndex;
            }
        }

        for(int digitIndex = firstDigitIndex + 1;  digitIndex < bank.Length; digitIndex++)
        {
            string digitString = bank[digitIndex].ToString();
            int digit = int.Parse(digitString);
            if (digit > secondDigit)
            {
                secondDigit = digit;
            }
        }

        return (firstDigit * 10) + secondDigit;
    }

    public long Choose12Batteries(string bank)
    {
        // Translate to a list of ints.
        List<int> intList = [];
        foreach(char digitChar in bank)
        {
            intList.Add(int.Parse(digitChar.ToString()));
        }

        string joltagesString = "";
        int batteriesAfter = 11;
        while (batteriesAfter >= 0)
        {
            RecursionState state = ChooseBattery(intList, batteriesAfter);
            joltagesString += state.maxNum.ToString();
            intList = state.shortenedBatteryList;
            batteriesAfter--;
        }

        return long.Parse(joltagesString);
    }

    private RecursionState ChooseBattery(List<int> batteryIntList, int batteriesAfter)
    {
        int bankLength = batteryIntList.Count;
        int maxNum = 0;
        int maxNumIndex = 0;
        for (int digitIndex = 0; digitIndex < bankLength - batteriesAfter; digitIndex++)
        {
            if (batteryIntList[digitIndex] > maxNum) 
            { 
                maxNum = batteryIntList[digitIndex];
                maxNumIndex = digitIndex;
            }
        }

        int batteriesRemoved = maxNumIndex + 1;
        List<int> shortenedBatteryList = batteryIntList.Slice(batteriesRemoved, bankLength - batteriesRemoved);

        return new(maxNum, shortenedBatteryList);
    }
}
