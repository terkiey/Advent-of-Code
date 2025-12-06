namespace AoC.Days;

internal record FreshRange(ulong Start, ulong End)
{
    public ulong Size
    {
        get
        {
            return (End - Start) + 1;
        }
    }
    public bool TryCombine(FreshRange otherRange, out FreshRange newRange)
    {
        newRange = this;
        if (this == otherRange)
        {
            return true;
        }

        if (Start <= otherRange.Start && otherRange.End <= End)
        {
            return true;
        }

        if (otherRange.Start <= Start && End <= otherRange.End)
        {
            newRange = otherRange;
            return true;
        }

        ulong newEnd = End;
        ulong newStart = Start;
        bool touched = false;
        if (Start <= otherRange.Start && otherRange.Start <= End)
        {
            newEnd = Math.Max(End, otherRange.End);
            touched = true;
        }

        else if (Start <= otherRange.End && otherRange.End <= End)
        {
            newStart = Math.Min(Start, otherRange.Start);
            touched = true;
        }


        newRange = new(newStart, newEnd);
        return touched;
    }

    public FreshRange Combine(FreshRange otherRange)
    {
        if (Start <= otherRange.Start && otherRange.End <= End)
        {
            return this;
        }

        if (otherRange.Start <= Start && End <= otherRange.End)
        {
            return otherRange;
        }

        ulong newEnd = End;
        ulong newStart = Start;
        if (Start <= otherRange.Start && otherRange.Start <= End)
        {
            newEnd = Math.Max(End, otherRange.End);
        }

        else if (Start <= otherRange.End && otherRange.End <= End)
        {
            newStart = Math.Min(Start, otherRange.Start);
        }

        return new FreshRange(newStart, newEnd);
    }

    public bool Contains(ulong ingredient)
    {
        if (ingredient < Start)
        {
            return false;
        }

        if (ingredient > End)
        {
            return false;
        }

        return true;
    }
}
