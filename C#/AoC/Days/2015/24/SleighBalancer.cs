namespace AoC.Days;

internal class SleighBalancer
{
    private readonly List<int> presents;
    private readonly int EqualBalanceWeightThreeWays;
    private readonly int EqualBalanceWeightFourWays;

    public SleighBalancer(string[] inputLines)
    {
        presents = [];
        foreach (string line in inputLines)
        {
            presents.Add(int.Parse(line));
        }

        EqualBalanceWeightThreeWays = presents.Sum() / 3;
        EqualBalanceWeightFourWays = (EqualBalanceWeightThreeWays * 3) / 4;
    }

    public long GetIdealBalanceQEThreeWays()
    {
        List<int[][]> balancedDistributions = [];
        int minPresents = int.MaxValue;

        List<int> remainingPresents = [.. presents];
        for (int presentCountOne = 1; presentCountOne <= remainingPresents.Count - 2;  presentCountOne++)
        {
            foreach (int[] presentsOne in GetCombinations(remainingPresents, presentCountOne))
            {
                if (presentsOne.Sum() != EqualBalanceWeightThreeWays)
                {
                    continue;
                }
                remainingPresents = [.. presents.Except(presentsOne)];

                for (int presentCountTwo = 1; presentCountTwo <= remainingPresents.Count - 1; presentCountTwo++)
                {
                    foreach (int[] presentsTwo in GetCombinations(remainingPresents, presentCountTwo))
                    {
                        if (presentsTwo.Sum() != EqualBalanceWeightThreeWays)
                        {
                            continue;
                        }
                        remainingPresents = [.. remainingPresents.Except(presentsTwo)];
                        int[][] balancedPresents = [presentsOne, presentsTwo, [.. remainingPresents]];
                        minPresents = minPresents > presentsOne.Length ? presentsOne.Length : minPresents;
                        balancedDistributions.Add(balancedPresents);
                    }
                }
            }
        }
        List<long> quantumEntanglements = [];
        List<int[][]> lowCountBalanced = [.. balancedDistributions.Where(dist => dist[0].Length == minPresents)];
        long lowestQE = long.MaxValue;
        foreach (int[][] distribution in lowCountBalanced)
        {
            long distQE = 1;
            foreach (long present in distribution[0])
            {
                distQE *= present;
            }
            lowestQE = Math.Min(lowestQE, distQE);
        }
        return lowestQE;
    }


    public long GetIdealBalanceQEFourWays()
    {
        List<int[][]> balancedDistributions = [];
        int minPresents = int.MaxValue;

        List<int> remainingPresents = [.. presents];
        for (int presentCountOne = 1; presentCountOne <= remainingPresents.Count - 3; presentCountOne++)
        {
            foreach (int[] presentsOne in GetCombinations(remainingPresents, presentCountOne))
            {
                if (presentsOne.Sum() != EqualBalanceWeightFourWays)
                {
                    continue;
                }
                remainingPresents = [.. presents.Except(presentsOne)];

                for (int presentCountTwo = 1; presentCountTwo <= remainingPresents.Count - 2; presentCountTwo++)
                {
                    foreach (int[] presentsTwo in GetCombinations(remainingPresents, presentCountTwo))
                    {
                        if (presentsTwo.Sum() != EqualBalanceWeightFourWays)
                        {
                            continue;
                        }
                        remainingPresents = [.. remainingPresents.Except(presentsTwo)];

                        for (int presentCountThree = 1; presentCountThree <= remainingPresents.Count - 1; presentCountThree++)
                        {
                            foreach (int[] presentsThree in GetCombinations(remainingPresents, presentCountThree))
                            {
                                if (presentsThree.Sum() != EqualBalanceWeightFourWays)
                                {
                                    continue;
                                }
                                remainingPresents = [.. remainingPresents.Except(presentsThree)];
                                int[][] balancedPresents = [presentsOne, presentsTwo, presentsThree, [.. remainingPresents]];
                                minPresents = minPresents > presentsOne.Length ? presentsOne.Length : minPresents;
                                balancedDistributions.Add(balancedPresents);
                            }
                        }
                    }
                }
            }
        }
        List<long> quantumEntanglements = [];
        List<int[][]> lowCountBalanced = [.. balancedDistributions.Where(dist => dist[0].Length == minPresents)];
        long lowestQE = long.MaxValue;
        foreach (int[][] distribution in lowCountBalanced)
        {
            long distQE = 1;
            foreach (long present in distribution[0])
            {
                distQE *= present;
            }
            lowestQE = Math.Min(lowestQE, distQE);
        }
        return lowestQE;
    }

    private IEnumerable<int[]> GetCombinations(List<int> itemList, int length)
    {
        int n = itemList.Count;
        if (length > n || length <= 0)
        {
            yield break;
        }

        var indices = Enumerable.Range(0, length).ToArray();
        while (true)
        {
            // Take current indices (starts at 0,1,2...) and yield that combination.
            var result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = itemList[indices[i]];
            }
            yield return result;

            // Point i2 at the rightmost non-maxed out index (maxed out given we only want to take ascending combinations, so lefter indices must be lower)
            int i2;
            for (i2 = length - 1; i2 >= 0 && indices[i2] == i2 + n - length; i2--)
            {

            }

            // If there is no non-maxed index, we are done.
            if (i2 < 0)
            {
                yield break;
            }

            // increment index at i2 to point to next object, reset indices to the right to be in ascending by one to start new cycle.
            indices[i2]++;
            for (int j = i2 + 1; j < length; j++)
            {
                indices[j] = indices[j - 1] + 1;
            }
        }
    }
}
