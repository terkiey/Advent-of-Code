namespace AoC.Days;

internal class JoltageState : IEquatable<JoltageState>
{
    private readonly int _hash;

    public readonly int[] ButtonPressCounts;

    public JoltageState(int[] buttonPressCounts)
    {
        ButtonPressCounts = buttonPressCounts;
        _hash = ComputeHash(ButtonPressCounts);
    }

    public IEnumerable<JoltageState> CreateNeighbours(int[][] buttons)
    {
        for (int buttonIndex = 0; buttonIndex < buttons.Length; buttonIndex++)
        {
            int[] newButtonCounts = new int[ButtonPressCounts.Length];
            Array.Copy(ButtonPressCounts, newButtonCounts, newButtonCounts.Length);

            newButtonCounts[buttonIndex]++;
            yield return new(newButtonCounts);
        }
    }

    public int[] ComputeJoltages(int[][] buttons, int joltageCount)
    {
        int[] joltages = new int[joltageCount];
        for (int buttonIndex = 0; buttonIndex < ButtonPressCounts.Length; buttonIndex++)
        {
            int pressCount = ButtonPressCounts[buttonIndex];
            if (pressCount == 0) { continue; }
            foreach (int joltageIndex in buttons[buttonIndex])
            {
                joltages[joltageIndex] += pressCount;
            }
        }
        return joltages;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as JoltageState);
    }

    public bool Equals(JoltageState? otherJoltageState)
    {
        if (otherJoltageState is null) return false;
        if (_hash != otherJoltageState._hash) return false;
        return ButtonPressCounts.AsSpan().SequenceEqual(otherJoltageState.ButtonPressCounts);
    }

    public override int GetHashCode() => _hash;

    private int ComputeHash(int[] stateData)
    {
        unchecked
        {
            int hash = 17;
            foreach (int number in stateData)
            {
                hash = (hash * 31) + number;
            }
            return hash;
        }
    }

    
}
