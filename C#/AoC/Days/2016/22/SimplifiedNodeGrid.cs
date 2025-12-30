using System.Drawing;

namespace AoC.Days;

internal class SimplifiedNodeGrid
{
    private HashSet<Point> blockedCoords = [];
    private Point EmptyNode;
    private Point GoalData;

    public void DeduceSimplifiedModel(GridNode[][] fullNetwork)
    {
        /* Simplify the node grid model as the puzzle suggests:
         * keep a pointer for the empty node, and the goal data.
         * put a blocker for every node that has fat data.
         */

        int maxCols = fullNetwork.Length;
        int maxRows = fullNetwork[0].Length;

        GoalData = new(fullNetwork.Length - 1, 0);
        int fatDataThreshold = fullNetwork[0][0].Used + fullNetwork[0][0].Available;

        for(int colIndex = 0; colIndex < maxCols; colIndex++)
        {
            GridNode[] nodeCol = fullNetwork[colIndex];
            for (int rowIndex = 0; rowIndex < nodeCol.Length; rowIndex++)
            {
                GridNode node = nodeCol[rowIndex];
                if (node.Used == 0)
                {
                    EmptyNode = new(node.Location[0], node.Location[1]);
                }

                if (node.Used > fatDataThreshold)
                {
                    blockedCoords.Add(new(node.Location[0], node.Location[1]));
                }
            }
        }
    }

    public int LeastStepsForTransfer()
    {
        // From analysing the fat nodes, it is clear they are all on y = 7 from x =14 to x=36. So the optimal solution is to simply move the goal data left over and over.
        // So this means the initial number of moves to bring the empty node next to the goal data (to its left, the swap over, and then repeatedly doing 4 swaps to get the empty node back on the left and swapping.
        // keep in mind you must go around the wall of fat data nodes (so must go to x = 13 and back up)

        // If I cared I'd make it path smartly but because I know what my data looks like I'm just going to hardcode the values in for the x moves and then in future maybe I'll implement a maze algorithm akin to day 17 for actually pathing around the fat nodes.

        int xMove = Math.Abs(EmptyNode.X - 13) + Math.Abs(13 - (GoalData.X - 1));
        int yMove = EmptyNode.Y; // Because goal data starts on y = 0.
        int initialSteps = xMove + yMove + 1;
        return initialSteps + ((GoalData.X - 1) * 5);
    }
}
