using System.IO.Enumeration;

namespace AoC.Days;

public record DayArgs(string filename, int year)
{
    // For when the test input uses a different parameter setting than the real input.
    public int RunParameter => FileRunParameter(year);

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

        if (year == 2017)
        {
            return filename switch
            {
                "day10input.txt" => 1,
                "day10testinput.txt" => 2,
                _ => 0,
            };
        }

        if (year == 2016)
        {
            return filename switch
            {
                "day08input.txt" => 1,
                "day08testinput.txt" => 2,

                "day13input.txt" => 1,
                "day13testinput.txt" => 2,

                "day18input.txt" => 1,
                "day18testinput.txt" => 2,

                "day20input.txt" => 1,
                "day20testinput.txt" => 2,

                "day21input.txt" => 1,
                "day21testinput.txt" => 2,
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
