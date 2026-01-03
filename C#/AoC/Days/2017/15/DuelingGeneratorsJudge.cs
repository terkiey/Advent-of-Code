using System.Runtime.ExceptionServices;

namespace AoC.Days;

internal class DuelingGeneratorsJudge
{
    public int Count { get; private set; } = 0;
    private long GenA { get; set; } = 0;
    private long GenB { get; set; } = 0;
    private long MultA { get; } = 16807;
    private long MultB { get; } = 48271;
    private long Modulo { get; } = 2147483647;

    public void GenSeeds(string[] inputLines)
    {
        GenA = long.Parse(inputLines[0].Split(' ')[^1]);
        GenB = long.Parse(inputLines[1].Split(' ')[^1]);
    }

    public void Duel(int duelLength)
    {
        Count = 0;
        int duels = 0;
        while (duels++ < duelLength)
        {
            GenA = (GenA * MultA) % Modulo;
            GenB = (GenB * MultB) % Modulo;

            if ((GenA & 0xffff) == (GenB & 0xffff))
            {
                Count++;
            }
        }
    }

    public void DuelTwo(int duelLength)
    {
        Count = 0;
        int duels = 0;
        while (duels++ < duelLength)
        {
            GenA = GetNext(GenA, MultA, 4);
            GenB = GetNext(GenB, MultB, 8);

            if ((GenA & 0xffff) == (GenB & 0xffff))
            {
                Count++;
            }
        }
    }

    // The modulo number is 2^31 - 1, which is a mersenne prime.
    // Therefore, 2^31 is equivalent to 1 (mod 2^31 - 1).
    // We also know that our number (which has from 0 to x bits , with x surely less than 62, based on how its calculated)
    // You can write X as (first 31 bits) + 2^31 * (remaining bits) as thats just how you construct a binary number.
    // Well given we know there will never be more than 31 more bits (we would have to multiply by another number on the order of 2^31...
    // We can just say that mod 2^31 -1 this will be equivalent to (first 32 bits) + (remaining bits)
    // M's binary rep is 1111... (31 times), so basically we can get the first 31 digits by doing value & M
    private long FastMersenneModulo(long value)
    {
        // BUT, this wasnt even faster in the end. Because the compiler already optimises modulo by constants into multiply-and-shift instructions!
        long firstDigits = value & Modulo;
        long lastDigits = value >> 31;
        long output = firstDigits + lastDigits;
        return output -= (output >= Modulo ? Modulo : 0);
    }

    private long GetNext(long value, long mult, long factorRequired)
    {
        value = (value * mult) % Modulo;
        // These bit tricks for modulo 2^n are apparently likely to be included in compiler automatically, but thought I'd implement to learn. Well it seems to save 5% so cant complain.
        if (factorRequired == 4)
        {
            while ((value & 3) != 0)
            {
                value = (value * mult) % Modulo;
            }
        }
        else if (factorRequired == 8)
        {
            while ((value & 7) != 0)
            {
                value = (value * mult) % Modulo;
            }
        }
        else
        {
            while (value % factorRequired != 0)
            {
                value = (value * mult) % Modulo;
            }
        }
            
        return value;
    }
}
