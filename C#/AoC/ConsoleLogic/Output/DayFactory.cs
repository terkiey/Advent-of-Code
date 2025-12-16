using AoC.Days;

namespace AoC.ConsoleLogic;

internal class DayFactory : IDayFactory
{
    public Dictionary<int, Func<IDay>> dayFactories2025 = new()
    {
        { 01, () => new Y2025Day01() },
        { 02, () => new Y2025Day02() },
        { 03, () => new Y2025Day03() },
        { 04, () => new Y2025Day04() },
        { 05, () => new Y2025Day05() },
        { 06, () => new Y2025Day06() },
        { 07, () => new Y2025Day07() },
        { 08, () => new Y2025Day08() },
        { 09, () => new Y2025Day09() },
        { 10, () => new Y2025Day10() },
        { 11, () => new Y2025Day11() },
        { 12, () => new Y2025Day12() }
    };

    public Dictionary<int, Func<IDay>> dayFactories2015 = new()
    {
        { 01, () => new Y2015Day01() },
        { 02, () => new Y2015Day02() },
        { 03, () => new Y2015Day03() },
        { 04, () => new Y2015Day04() },
        { 05, () => new Y2015Day05() },
        { 06, () => new Y2015Day06() },
        { 07, () => new Y2015Day07() },
        { 08, () => new Y2015Day08() },
        { 09, () => new Y2015Day09() },
        { 10, () => new Y2015Day10() },
        { 11, () => new Y2015Day11() },
        { 12, () => new Y2015Day12() },
        { 13, () => new Y2015Day13() },
        { 14, () => new Y2015Day14() },
        { 15, () => new Y2015Day15() },
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
