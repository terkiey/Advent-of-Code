using AoC.Days;

namespace AoC.ConsoleLogic;

internal class DayFactory : IDayFactory
{
    public Dictionary<int, Dictionary<int, Func<IDay>>> _factories = new();

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

    public Dictionary<int, Func<IDay>> dayFactories2017 = new()
    {
        { 01, () => new Y2017Day01() },
        { 02, () => new Y2017Day02() },
        { 03, () => new Y2017Day03() },
    };

    public Dictionary<int, Func<IDay>> dayFactories2016 = new()
    {
        { 01, () => new Y2016Day01() },
        { 02, () => new Y2016Day02() },
        { 03, () => new Y2016Day03() },
        { 04, () => new Y2016Day04() },
        { 05, () => new Y2016Day05() },
        { 06, () => new Y2016Day06() },
        { 07, () => new Y2016Day07() },
        { 08, () => new Y2016Day08() },
        { 09, () => new Y2016Day09() },
        { 10, () => new Y2016Day10() },
        { 11, () => new Y2016Day11() },
        { 12, () => new Y2016Day12() },
        { 13, () => new Y2016Day13() },
        { 14, () => new Y2016Day14() },
        { 15, () => new Y2016Day15() },
        { 16, () => new Y2016Day16() },
        { 17, () => new Y2016Day17() },
        { 18, () => new Y2016Day18() },
        { 19, () => new Y2016Day19() },
        { 20, () => new Y2016Day20() },
        { 21, () => new Y2016Day21() },
        { 22, () => new Y2016Day22() },
        { 23, () => new Y2016Day23() },
        { 24, () => new Y2016Day24() },
        { 25, () => new Y2016Day25() },
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
        { 16, () => new Y2015Day16() },
        { 17, () => new Y2015Day17() },
        { 18, () => new Y2015Day18() },
        { 19, () => new Y2015Day19() },
        { 20, () => new Y2015Day20() },
        { 21, () => new Y2015Day21() },
        { 22, () => new Y2015Day22() },
        { 23, () => new Y2015Day23() },
        { 24, () => new Y2015Day24() },
        { 25, () => new Y2015Day25() },
    };

    public DayFactory() 
    {
        InitialiseFactories();
    }

    public Dictionary<int, Func<IDay>> GetFactory(int year)
    {
        return _factories[year];
    }

    private void InitialiseFactories()
    {
        _factories.Add(2015, dayFactories2015);
        _factories.Add(2016, dayFactories2016);
        _factories.Add(2017, dayFactories2017);
        _factories.Add(2025, dayFactories2025);

    }
}
