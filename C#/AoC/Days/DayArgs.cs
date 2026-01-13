using System.IO.Enumeration;

namespace AoC.Days;

public record DayArgs(string filename, int year)
{
    // For when the test input uses a different parameter setting than the real input.
    public int RunParameter => FileRunParameter(year);

    private int FileRunParameter(int year)
    {
        if (filename[^13..] == "testinput.txt")
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }
}
