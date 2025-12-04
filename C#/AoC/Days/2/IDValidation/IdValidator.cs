namespace AoC;

internal class IdValidator : IIdValidator
{
    public IdValidator() { }

    public bool ValidateIdPartOne(string id)
    {
        int digits = id.Length;
        // id must be even digits to be something repeated twice.
        if (digits % 2 != 0)
        {
            return true;
        }

        if (IsRepeatedChunk(id, digits/2))
        {
            return false;
        }

        return true;
    }

    public bool ValidateIdPartTwo(string id)
    {
        int digits = id.Length;
        // Determine possible repetition quantities (clean factors of digits).
        List<int> factors = [];
        for (int divisor = 2; divisor <= digits; divisor++)
        {
            if (digits % divisor == 0)
            {
                factors.Add(divisor);
            }
        }

        // Check for those repetition types one by one, if any are true, immediately report the ID as invalid.
        foreach (int factor in factors)
        {
            if (IsRepeatedChunk(id, id.Length / factor))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsRepeatedChunk(string id, int chunkLength)
    {
        int repetitionsNeeded = id.Length / chunkLength;
        string repeatedFragment = id.Substring(0, chunkLength);
        for (int fragmentIndex = 1; fragmentIndex < repetitionsNeeded; fragmentIndex++)
        {
            if (repeatedFragment != id.Substring(fragmentIndex * chunkLength, chunkLength))
            {
                return false;
            }
        }

        return true; ;
    }
}
