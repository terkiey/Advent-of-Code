using System.Text.RegularExpressions;

namespace AoC.Days;

internal class Y2017Day07 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        DiscTowerManager manager = new();
        manager.ConstructTowers(inputLines);
        AnswerOne = manager.BaseTower();
        AnswerTwo = manager.FixedWeightToBalance().ToString();
    }
}
