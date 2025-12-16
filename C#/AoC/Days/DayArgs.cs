using System.IO.Enumeration;

namespace AoC.Days;

public record DayArgs(string filename, int year)
{
    // For when the test input uses a different parameter setting than the real input.
    public int runParameter => FileRunParameter(year);

    private int FileRunParameter(int year)
    {
        if (year == 2025)
        {
            return filename switch
            {
                "day08input.txt" => 1000,
                "day08testinput.txt" => 10,
                _ => 0,
            };
        }

        if (year == 2015)
        {
            return filename switch
            {
                "day14input.txt" => 2503,
                "day14testinput.txt" => 1000,
                _ => 0
            };
        }

        return 0;
    }
}
