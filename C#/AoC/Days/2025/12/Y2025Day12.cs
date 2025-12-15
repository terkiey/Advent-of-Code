namespace AoC.Days;

internal class Y2025Day12: Day
{
    protected override void RunLogic(string[] lines)
    {
        IPresentAndTreeProblemParser parser = new PresentAndTreeProblemParser(lines);
        PresentShape[] presentShapes = parser.GetPresents();
        TreeProblem[] treeProblems = parser.GetProblems();

        ITreeProblemSolver solver = new TreeProblemSolver(presentShapes, treeProblems);
        int problemCount = treeProblems.Length;
        int AnswerOneNum = 0;
        for (int i = 0; i < problemCount; i++)
        {
            if (solver.Solvable(i))
            {
                AnswerOneNum++;
            }
        }
        AnswerOne = AnswerOneNum.ToString();
    }
}
