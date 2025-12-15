using AoC.Days;
using System.Runtime.InteropServices.Marshalling;

namespace AoC.ConsoleLogic;

internal class DayFactory : IDayFactory
{
    public Dictionary<int, Func<IDay>> dayFactories2025 = new()
    {
        { 1, () => new Y2025Day1() },
        { 2, () => new Y2025Day2() },
        { 3, () => new Y2025Day3() },
        { 4, () => new Y2025Day4() },
        { 5, () => new Y2025Day5() },
        { 6, () => new Y2025Day6() },
        { 7, () => new Y2025Day7() },
        { 8, () => new Y2025Day8() },
        { 9, () => new Y2025Day9() },
        { 10, () => new Y2025Day10() },
        { 11, () => new Y2025Day11() },
        { 12, () => new Y2025Day12() }
    };

    public Dictionary<int, Func<IDay>> dayFactories2015 = new()
    {
        { 1, () => new Y2015Day1() },
    };

    public DayFactory() { }

    public Dictionary<int, Func<IDay>> GetFactory(int year)
    {
        switch (year)
        {
            case 2015:
                return dayFactories2015;

            case 2025:
                return dayFactories2025;

            default:
                throw new Exception("No year for factories specified");
        }
    }
}
