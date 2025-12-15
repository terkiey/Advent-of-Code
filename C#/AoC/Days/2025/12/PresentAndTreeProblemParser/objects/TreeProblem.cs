using System.Drawing;

namespace AoC.Days;

internal record TreeProblem(int width, int height, int[] presentCounts)
{
    public bool IsEasierThan(TreeProblem otherProblem)
    {
        if(Math.Min(width, height) < Math.Min(otherProblem.width, otherProblem.height))
        {
            return false;
        }

        if(Math.Max(width, height) < Math.Max(otherProblem.width, otherProblem.height))
        {
            return false;
        }

        for (int presentIndex = 0; presentIndex < presentCounts.Length; presentIndex++)
        {
            if (presentCounts[presentIndex] > otherProblem.presentCounts[presentIndex])
            {
                return false;
            }
        }

        return true;
    }
}
