using System.Text.RegularExpressions;

namespace AoC.Days;

internal class Y2017Day06 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        string banksString = inputLines[0];
        MatchCollection blockCounts = Regex.Matches(banksString, @"(\d+)");
        int index = 0;
        int[] banksArray = new int[blockCounts.Count];
        foreach (Match match in blockCounts)
        {
            banksArray[index++] = int.Parse(match.Value);
        }

        int[] currentBanks = banksArray.ToArray();
        HashSet<MemoryBankArray> seenStates = [];
        List<MemoryBankArray> stateList = [];
        int steps = 0;
        while (true)
        {
            if (!seenStates.Add(new(currentBanks)))
            {
                break;
            }
            stateList.Add(new(currentBanks));

            int maxValue = -1;
            int maxIndex = -1;
            for (index = 0; index < currentBanks.Length; index++)
            {
                if (currentBanks[index] > maxValue)
                {
                    maxValue = currentBanks[index];
                    maxIndex = index;
                }
            }

            currentBanks[maxIndex] = 0;
            index = (maxIndex + 1) % currentBanks.Length;
            while (maxValue > 0)
            {
                currentBanks[index++]++;
                maxValue--;
                index %= currentBanks.Length;
            }
            steps++;
        }
        AnswerOne = steps.ToString();
        int loopLength = stateList.Count - stateList.IndexOf(new(currentBanks));
        AnswerTwo = loopLength.ToString();
    }
}
