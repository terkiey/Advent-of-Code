using System.Numerics;

namespace AoC.Days;

internal class MemoryBankArray : IEquatable<MemoryBankArray>
{
    private readonly int[] Banks;
    public int Length => Banks.Length;

    public MemoryBankArray(int[] banks)
    {
        Banks = banks.ToArray();
    }

    public bool Equals(MemoryBankArray? otherBankArray)
    {
        if (ReferenceEquals(this, otherBankArray) || otherBankArray is null)
        {
            return true;
        }
        return this.Banks.SequenceEqual(otherBankArray.Banks);
    }

    public override bool Equals(object? obj)
    {
        return obj is MemoryBankArray other && Equals(other);
    }

    public static bool operator ==(MemoryBankArray a, MemoryBankArray b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(MemoryBankArray a, MemoryBankArray b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        HashCode hash = new();
        foreach (int bank in Banks)
        {
            hash.Add(bank);
        }
        return hash.ToHashCode();
    }

    public int this[int index]
    {
        get { return Banks[index]; }
    }
}
