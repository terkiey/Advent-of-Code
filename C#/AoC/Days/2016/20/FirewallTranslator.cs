namespace AoC.Days;

internal class FirewallTranslator
{
    private List<uint[]> Ranges = [];
    public void InputRanges(string[] inputLines)
    {
        foreach(string rangeString in inputLines)
        {
            string[] splitRange = rangeString.Split('-');
            uint[] range = [uint.Parse(splitRange[0]), uint.Parse(splitRange[1])];
            Ranges.Add(range);
        }
    }

    public uint LowestValidIp()
    {
        if (Ranges.Count == 0)
        {
            return 0;
        }

        CombineAndSortRanges();
        if (Ranges[0][0] > 0)
        {
            return 0;
        }
        return Ranges[0][1] + 1;
    }

    public long TotalAllowedIps(uint maxIpVal)
    {
        long allowedIpsCount = maxIpVal + 1L;
        foreach (uint[] range in Ranges)
        {
            uint ipsBlocked = range[1] - range[0] + 1;
            allowedIpsCount -= ipsBlocked;
        }
        return allowedIpsCount;
    }

    private void CombineAndSortRanges()
    {
        // Grab a range, compare to all other ranges later in the list than itself, if nothing got condensed, do the same for next range, until second last is processed.
        // If anything condensed, then start again from the start.
        bool condensedSomething = true;
        while (condensedSomething)
        {
            condensedSomething = false;
            List<uint[]> rangeRefsCopy = Ranges.ToList();
            for (int firstRangeIndex = 0; firstRangeIndex < rangeRefsCopy.Count - 1; firstRangeIndex++)
            {
                uint[] rangeOne = rangeRefsCopy[firstRangeIndex];
                for (int secondRangeIndex = firstRangeIndex + 1; secondRangeIndex < rangeRefsCopy.Count; secondRangeIndex++)
                {
                    uint[] rangeTwo = rangeRefsCopy[secondRangeIndex];
                    if (rangeOne == rangeTwo)
                    {
                        continue;
                    }

                    if (TryCondense(rangeOne, rangeTwo, out uint[] condensedRange))
                    {
                        Ranges.Remove(rangeOne);
                        Ranges.Remove(rangeTwo);
                        Ranges.Insert(0, condensedRange);
                        condensedSomething = true;
                        break;
                    }
                }

                if (condensedSomething)
                {
                    break;
                }
            }
        }
        // Once done, sort by range start or end, because condensed either works.
        Ranges.Sort((r1, r2) => r1[0].CompareTo(r2[0]));
    }

    private bool TryCondense(uint[] currentRange, uint[] otherRange, out uint[] condensedRange)
    {
        condensedRange = [];
        if ((currentRange[0] <= otherRange[0] && otherRange[0] <= currentRange[1]) ||
                (currentRange[0] <= otherRange[1] && otherRange[1] <= currentRange[1]) ||
                (otherRange[0] <= currentRange[0] && currentRange[0] <= otherRange[1]) ||
                (otherRange[0] <= currentRange[1] && currentRange[1] <= otherRange[1]) ||
                (Math.Abs((long)otherRange[0] - (long)currentRange[0]) < 2) ||
                (Math.Abs((long)otherRange[1] - (long)currentRange[0]) < 2) ||
                (Math.Abs((long)otherRange[0] - (long)currentRange[1]) < 2) ||
                (Math.Abs((long)otherRange[1] - (long)currentRange[1]) < 2))
        {
            condensedRange = [Math.Min(currentRange[0], otherRange[0]), Math.Max(currentRange[1], otherRange[1])];
            uint test = condensedRange[0];
            if (test == 0)
            {
                return true;
            }
            return true;
        }
        return false;
    }
}
