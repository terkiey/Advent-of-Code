namespace AoC.Days;

internal class Y2016Day11 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        IsotopeMoverSolver solver = new();
        AnswerOne = solver.MinimumMoves().ToString();
        AnswerTwo = solver.MinimumMovesPartTwo().ToString();
    }
}
