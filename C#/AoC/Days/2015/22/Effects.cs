using System.Xml.Linq;

namespace AoC.Days;

internal enum Effect
{
    Shield = 0,
    Poison = 1,
    Recharge = 2,
    HardMode = 3,
}

internal class EffectInstance
{
    public Effect Type;
    public int RemainingTurns;

    public EffectInstance(Effect type, int remainingTurns)
    {
        Type = type;
        RemainingTurns = remainingTurns;
    }

    public bool Equals(EffectInstance other)
    {
        if (ReferenceEquals(this, other)) return true;
        return Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        if (obj is null || obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((EffectInstance)obj);
    }

    public static bool operator ==(EffectInstance lhs, EffectInstance rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(EffectInstance lhs, EffectInstance rhs)
    {
        return !lhs.Equals(rhs);
    }

    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(Type);
        return hash.ToHashCode();
    }
}
