namespace AoC.Days;

internal class SpinLockSimulator
{
    // Actually simulate for part one.
    private List<int> Values = [0];
    private int Pointer = 0;
    private int Cycle = 0;

    public int PartOneValue { get; private set; }
    public int PartTwoValue { get; private set; }

    public void SimulatePartOne(string stepSeedString)
    {
        int totalCycles = 2017;
        int stepSize = int.Parse(stepSeedString);
        while (Cycle < totalCycles)
        {
            Pointer += stepSize;
            Pointer %= ++Cycle;
            Values.Insert(++Pointer, Cycle);
        }

        PartOneValue = Values[Pointer + 1];
        Values.Clear();
        Pointer = 0;
        Cycle = 0;
    }

    // Dont bother keeping all the values (50 million ints in a dynamic array is slow), we can deduce that the value after 0 is always going to be associated to the cycle when the pointer last landed on 0.
    public void SimulatePartTwo(string stepSeedString)
    {
        int totalCycles = 50000000;
        int stepSize = int.Parse(stepSeedString);
        int firstNonZero = -1;
        while (Cycle < stepSize && Cycle < totalCycles)
        {
            Pointer += stepSize;
            Pointer %= ++Cycle;
            if (++Pointer == 1)
            {
                firstNonZero = Cycle;
            }
        }

        // Once the list is long enough, we know we can simplify the modulo operation for a time-save.
        while (Cycle < totalCycles)
        {
            Pointer += stepSize;
            if (Pointer >= ++Cycle)
            {
                Pointer -= Cycle;
            }
            if (++Pointer == 1)
            {
                firstNonZero = Cycle;
            }
        }

        PartTwoValue = firstNonZero;
    }
}
