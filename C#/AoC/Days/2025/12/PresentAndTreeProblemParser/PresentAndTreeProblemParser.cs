using System.Drawing;

namespace AoC.Days;

internal class PresentAndTreeProblemParser : IPresentAndTreeProblemParser
{
    private const int presentCount = 6;
    private PresentShape[] PresentShapes = [];
    private TreeProblem[] TreeProblems = [];

    public PresentAndTreeProblemParser(string[] inputLines)
    {
        ParseInput(inputLines);
    }

    public PresentShape[] GetPresents()
    {
        return PresentShapes;
    }

    public TreeProblem[] GetProblems()
    {
        return TreeProblems;
    }

    private void ParseInput(string[] inputLines)
    {
        List<string> presentsLines = [];
        List<string> treeProblemsLines = [];
        bool treeProblemSection = false;
        for (int lineIndex = 0; lineIndex < inputLines.Length; lineIndex++)
        {
            string line = inputLines[lineIndex];

            if (treeProblemSection)
            {
                treeProblemsLines.Add(line);
            }
            else if (line.Contains('x'))
            {
                treeProblemsLines.Add(line);
            }
            else
            {
                presentsLines.Add(line);
            }
        }
        PresentShapes = ParsePresents(presentsLines);
        TreeProblems = ParseTreeProblems(treeProblemsLines);
    }

    private PresentShape[] ParsePresents(List<string> presentsSection)
    {
        PresentShape[] presentShapes = new PresentShape[presentCount];
        for (int presentIndex = 0; presentIndex < presentCount; presentIndex++)
        {
            int startLineIndex = (presentIndex * 5) + 1;
            int endLineIndex = (presentIndex * 5) + 3;
            string[] presentLines = new string[3];
            for (int lineIndex = startLineIndex; lineIndex <= endLineIndex; lineIndex++)
            {
                string line = presentsSection[lineIndex];
                presentLines[(lineIndex % 5) - 1] = line;
            }
            presentShapes[presentIndex] = ParsePresent(presentLines);
        }
        return presentShapes;
    }

    private PresentShape ParsePresent(string[] presentLines)
    {
        bool[,] presentArea = new bool[3, 3];
        for (int rowIndex = 0; rowIndex < 3; rowIndex++)
        {
            string row = presentLines[rowIndex];
            for (int colIndex = 0; colIndex < 3; colIndex++)
            {
                char presentChar = row[colIndex];
                presentArea[colIndex, rowIndex] = presentChar == '#' ? true : false;
            }
        }
        return new(presentArea);
    }

    private TreeProblem[] ParseTreeProblems(List<string> treeProblemsSection)
    {
        TreeProblem[] treeProblems = new TreeProblem[treeProblemsSection.Count];
        for (int lineIndex = 0; lineIndex < treeProblemsSection.Count; lineIndex++)
        {
            string[] splitLine = treeProblemsSection[lineIndex].Split(':');
            int[] splitArea = splitLine[0].Split('x')
                                          .Select(s => int.Parse(s))
                                          .ToArray();
            int[] presentCounts = splitLine[1].Remove(0,1)
                                        .Split(' ')
                                        .Select(s => int.Parse(s))
                                        .ToArray();
            treeProblems[lineIndex] = new(splitArea[0], splitArea[1], presentCounts);
        }
        return treeProblems;
    }
}
