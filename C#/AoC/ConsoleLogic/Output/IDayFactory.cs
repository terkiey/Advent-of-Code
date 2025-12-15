using AoC.Days;

namespace AoC.ConsoleLogic;

internal interface IDayFactory
{
    Dictionary<int, Func<IDay>> GetFactory(int year);
}
