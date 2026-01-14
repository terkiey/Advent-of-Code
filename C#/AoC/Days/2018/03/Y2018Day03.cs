using System.Drawing;

namespace AoC.Days;

internal class Y2018Day03 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        HashSet<Point> ClaimedSqInches = [];
        HashSet<Point> MultiClaimedSqInches = [];
        foreach (string claim in inputLines)
        {
            string[] splitClaim = claim.Split(' ');
            int leftGap = int.Parse(splitClaim[2].Split(',')[0]);
            int topGap = int.Parse(splitClaim[2].Split(',')[1][..^1]);
            int[] dimensions = new int[2]
            {
                int.Parse(splitClaim[^1].Split('x')[0]),
                int.Parse(splitClaim[^1].Split('x')[1])
            };

            for (int x = 0; x < dimensions[0]; x++)
            {
                int xCo = leftGap + x;
                for (int y = 0; y < dimensions[1]; y++)
                {
                    int yCo = topGap + y;
                    Point claimSquare = new Point(xCo, yCo);
                    if(!ClaimedSqInches.Add(claimSquare))
                    {
                        MultiClaimedSqInches.Add(claimSquare);
                    }
                }
            }
        }
        AnswerOne = MultiClaimedSqInches.Count.ToString();

        foreach (string claim in inputLines)
        {
            string[] splitClaim = claim.Split(' ');
            int leftGap = int.Parse(splitClaim[2].Split(',')[0]);
            int topGap = int.Parse(splitClaim[2].Split(',')[1][..^1]);
            int[] dimensions = new int[2]
            {
                int.Parse(splitClaim[^1].Split('x')[0]),
                int.Parse(splitClaim[^1].Split('x')[1])
            };

            bool overlap = false;
            for (int x = 0; x < dimensions[0]; x++)
            {
                int xCo = leftGap + x;
                for (int y = 0; y < dimensions[1]; y++)
                {
                    int yCo = topGap + y;
                    Point claimSquare = new Point(xCo, yCo);
                    if (MultiClaimedSqInches.Contains(claimSquare))
                    {
                        overlap = true;
                    }
                }
            }

            if (!overlap)
            {
                AnswerTwo = splitClaim[0][1..];
            }
        }
    }
}
