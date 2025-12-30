using System.Text.RegularExpressions;

namespace AoC.Days;

internal partial class NodeGridComputer
{
    public GridNode[][] _network = [];

    public void DeduceState(string[] inputLines)
    {
        // Deduce state of the network from the input lines, store in this class.

        // Parse all nodes into a list, then once we know the network size, initialise the network and assign the nodes to their correct spot.
        List<GridNode> nodes = [];
        int maxX = 0;
        int maxY = 0;
        foreach (string line in inputLines)
        {
            if (line[0] != '/')
            {
                continue;
            }

            GridNode node = ParseNode(line);
            nodes.Add(node);
            maxX = maxX > node.Location[0] ? maxX : node.Location[0];
            maxY = maxY > node.Location[1] ? maxY : node.Location[1];
        }

        int totalCols = maxX + 1;
        int totalRows = maxY + 1;
        _network = new GridNode[totalCols][];
        for (int colIndex = 0; colIndex < totalCols;  colIndex++)
        {
            _network[colIndex] = new GridNode[totalRows];
        }

        foreach (GridNode node in nodes)
        {
            _network[node.Location[0]][node.Location[1]] = node;
        }
    }

    public int ViablePairs()
    {
        /* Answer part one's question: 
         * How many pairs of nodes (A,B) exist such that A has nonzero used space, B has enough free space to contain all of A's used space, and A and B are different nodes?
         */

        // For each node in turn, grab it, and compare to all other LATER nodes and add to the pair count for each successful comparison, add twice if viable both ways.
        int viablePairCount = 0;
        for (int colIndexOne = 0; colIndexOne < _network.Length; colIndexOne++)
        {
        GridNode[] nodeCol = _network[colIndexOne];
            for (int rowIndexOne = 0; rowIndexOne < nodeCol.Length; rowIndexOne++)
            {
                GridNode firstNode = nodeCol[rowIndexOne];
                for (int colIndexTwo = colIndexOne; colIndexTwo < _network.Length; colIndexTwo++)
                {
                    GridNode[] nodeColTwo = _network[colIndexTwo];
                    for (int rowIndexTwo = 0; rowIndexTwo < nodeColTwo.Length; rowIndexTwo++)
                    {
                        // If we are in the same column, read from the next node. Otherwise start from the top.
                        if (rowIndexTwo == 0 && colIndexTwo == colIndexOne)
                        {
                            rowIndexTwo += rowIndexOne + 1;
                        }
                        if (rowIndexTwo >= nodeColTwo.Length)
                        {
                            continue;
                        }
                        GridNode secondNode = nodeColTwo[rowIndexTwo];
                        if (ViablePair(firstNode, secondNode))
                        {
                            viablePairCount++;
                        }
                        if (ViablePair(secondNode, firstNode))
                        {
                            viablePairCount++;
                        }
                    }
                }
            }
        }
        return viablePairCount;
    }

    private bool ViablePair(GridNode firstNode, GridNode secondNode)
    {
        if (firstNode.Used != 0 && firstNode.Used <= secondNode.Available)
        {
            return true;
        }
        return false;
    }

    private GridNode ParseNode(string line)
    {
        // Parse the string for a line into a GridNode object then return it.
        Match match = GridNodeParser().Match(line);
        int xVal = int.Parse(match.Groups[1].Value);
        int yVal = int.Parse(match.Groups[2].Value);
        int used = int.Parse(match.Groups[3].Value);
        int available = int.Parse(match.Groups[4].Value);

        int[] coords = [xVal, yVal];
        return new(used, available, coords);
    }

    [GeneratedRegex(@"x(\d+)-y(\d+)\s+\d+T\s+(\d+)T\s+(\d+)")]
    private static partial Regex GridNodeParser();
}
