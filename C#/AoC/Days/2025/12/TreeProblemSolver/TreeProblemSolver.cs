namespace AoC.Days;

internal class TreeProblemSolver : ITreeProblemSolver
{
    private readonly TreeProblem[] _treeProblems = [];
    private readonly PresentShape[] _presentShapes = [];
    private readonly Dictionary<int, int> _presentSpaceTaken = [];
    private readonly Dictionary<int, bool> _problemSolvable = [];
    
    public TreeProblemSolver(PresentShape[] presentShapes, TreeProblem[] treeProblems)
    {
        _presentShapes = presentShapes;
        _treeProblems = treeProblems;
        for (int presentIndex = 0; presentIndex < _presentShapes.Length;  presentIndex++)
        {
            PresentShape presentShape = _presentShapes[presentIndex];
            _presentSpaceTaken.Add(presentIndex, CountArea(presentShape.presentArea));
        }
    }

    public bool Solvable(int problemIndex)
    {
        return TrySolve(problemIndex);
    }

    private bool TrySolve(int problemIndex)
    {
        if (EasyFails(problemIndex))
        {
            _problemSolvable.Add(problemIndex, false);
            return false;
        }

        if (EasyWins(problemIndex))
        {
            _problemSolvable.Add(problemIndex, true);
            return true;
        }

        bool isSolvable = ExtremelyComplicatedPackingAlgorithm(problemIndex);
        _problemSolvable.Add(problemIndex, isSolvable);
        return isSolvable;
    }

    private bool EasyFails(int problemIndex)
    {
        TreeProblem problem = _treeProblems[problemIndex];
        return EasyFails(problem);
    }

    private bool EasyFails(TreeProblem problem)
    {
        if (!IsEnoughArea(problem))
        {
            return true;
        }

        foreach(var treeProblemSolvableKvp in _problemSolvable)
        {
            TreeProblem treeProblem = _treeProblems[treeProblemSolvableKvp.Key];
            if (treeProblem.IsEasierThan(problem) && treeProblemSolvableKvp.Value == false)
            {
                return true;
            }
        }
        return false;
    }

    private bool EasyWins(int problemIndex)
    {
        TreeProblem problem = _treeProblems[problemIndex];
        return EasyWins(problem);
    }

    private bool EasyWins(TreeProblem problem)
    {
        if (CanDumbTile(problem))
        {
            return true;
        }

        foreach (var treeProblemSolvableKvp in _problemSolvable)
        {
            TreeProblem treeProblem = _treeProblems[treeProblemSolvableKvp.Key];
            if (problem.IsEasierThan(treeProblem) && treeProblemSolvableKvp.Value == true)
            {
                return true;
            }
        }
        return false;
    }

    private bool CanDumbTile(TreeProblem problem)
    {
        int problemDumbTiles = (problem.width / 3) * (problem.height / 3);
        int totalPresentCount = problem.presentCounts.Sum();
        if (totalPresentCount <= problemDumbTiles)
        {
            return true;
        }
        return false;
    }

    private bool IsEnoughArea(TreeProblem problem)
    {
        int presentsSpaceTaken = 0;
        for (int presentIndex = 0; presentIndex < _presentShapes.Length; presentIndex++)
        {
            presentsSpaceTaken += problem.presentCounts[presentIndex] * _presentSpaceTaken[presentIndex];
        }

        return presentsSpaceTaken < problem.height * problem.width;
    }

    private int CountArea(bool[,] presentArea)
    {
        int count = 0;
        int rows = presentArea.GetLength(0);
        int cols = presentArea.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (presentArea[row, col])
                {
                    count++;
                }
            }
        }
        return count;
    }

    private bool ExtremelyComplicatedPackingAlgorithm(int problemIndex)
    {
        return false;
    }
}
