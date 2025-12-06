namespace AoC.Days;
internal class Day6 : Day
{
    protected override void RunLogic(string[] lines)
    {
        IEquationBuilder equationBuilder = new EquationBuilder();
        IEquationSolver equationSolver = new EquationSolver();

        List<Equation> equations = equationBuilder.BuildEquations(lines);
        List<ulong> solutions = equationSolver.SolveEquations(equations);

        ulong AnswerOneNum = 0;
        foreach (ulong answer in solutions)
        {
            AnswerOneNum += answer;
        }
        AnswerOne = AnswerOneNum.ToString();

        List<Equation> cephalopodEquations = equationBuilder.BuildCephalopodEquations(lines);
        List<ulong> cephalopodSolutions = equationSolver.SolveEquations(cephalopodEquations);

        ulong AnswerTwoNum = 0;
        foreach (ulong answer in cephalopodSolutions)
        {
            AnswerTwoNum += answer;
        }
        AnswerTwo = AnswerTwoNum.ToString();
    }
}
