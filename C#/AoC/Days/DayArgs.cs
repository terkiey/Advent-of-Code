using System.IO.Enumeration;

namespace AoC.Days;

public record DayArgs(string filename, int year)
{
    // For when the test input uses a different parameter setting than the real input.
    public int runParameter => FileRunParameter();

    private int FileRunParameter()
    {
       switch (filename)
       {
            case "day08input.txt":
                return 1000;

            case "day08testinput.txt":
                return 10;

            default:
                return 0;
       }
    }
}
