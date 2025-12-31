using System.Diagnostics.Tracing;
using System.Drawing;
using System.Net;
using System.Numerics;

namespace AoC.Days;

internal class AirDuctMazer
{
    // Maze Details
    private bool[][] _maze = [];
    private List<Point> _destinations = [];
    private Point _startPosition = new();

    // Shortest Paths
    private int[][] shortestPaths = [];

    // Path bitmasks with given endpoint and shortest length to achieve
    private int[][] bitmaskShortestPaths = [];

    // Construct maze from input
    public void ConstructMaze(string[] inputLines)
    {
        _maze = new bool[inputLines.Length][];
        for (int lineIndex = 0; lineIndex < inputLines.Length; lineIndex++)
        {
            _maze[lineIndex] = new bool[inputLines[0].Length];
            string line = inputLines[lineIndex];
            for (int cellIndex = 0; cellIndex < _maze[lineIndex].Length; cellIndex++)
            {
                if (line[cellIndex] == '#')
                {
                    _maze[lineIndex][cellIndex] = true;
                }
                else
                {
                    _maze[lineIndex][cellIndex] = false;
                }

                if (char.IsAsciiDigit(line[cellIndex]))
                {
                    if (line[cellIndex] == '0')
                    {
                        _startPosition = new(lineIndex, cellIndex);
                    }
                    else
                    {
                        _destinations.Add(new(lineIndex, cellIndex));
                    }
                }
            }
        }
    }

    public int[] FewestStepsVisitAllNumbers()
    {
        int[] output = new int[2];
        CalculateShortestPaths();
        CalculateBitmaskShortestPaths();
        output[0] = GetFastestTraversalSteps();
        output[1] = GetFastestTraversalAndReturnSteps();
        return output;
    }

    // Find shortest path length in the maze between all the destinations (and the start position)
    private void CalculateShortestPaths()
    {
        List<Point> allNodes = GetAllNodes();
        InitialiseShortestPathArray();
        for (int indexOne = 0; indexOne < allNodes.Count - 1;  indexOne++)
        {
            Point PointOne = allNodes[indexOne];
            for (int indexTwo = indexOne + 1; indexTwo < allNodes.Count; indexTwo++)
            {
                Point PointTwo = allNodes[indexTwo];
                int indexOffset = indexTwo - indexOne - 1;
                shortestPaths[indexOne][indexOffset] = CalculateShortestPath(PointOne, PointTwo);
            }
        }
    }

    // This ended up not being needed
    // If we can make a path via the nodes we have already pathed to, that is the upper bound length.
    // e.g. If we can go 1 -> 0 -> 2, then that length is the maximum distance for 1 -> 2
    private int GetUpperBoundPath(int indexOne, int indexTwo)
    {
        int upperBound = int.MaxValue;
        List<Point> allNodes = GetAllNodes();
        for (int otherIndex = 0; otherIndex < allNodes.Count; otherIndex++)
        {
            if (otherIndex == indexOne || otherIndex == indexTwo)
            {
                continue;
            }

            int arcOne = GetShortestPath(indexOne, otherIndex);
            int arcTwo = GetShortestPath(indexTwo, otherIndex);
            if (arcOne != int.MaxValue && arcTwo != int.MaxValue)
            {
                upperBound = arcOne + arcTwo < upperBound ? arcOne + arcTwo : upperBound;
            }
        }
        return upperBound;
    }

    // One source of truth, so the shortest path array holds paths to higher index locations, otherwise ask in reverse.
    private void InitialiseShortestPathArray()
    {
        List<Point> allNodes = GetAllNodes();
        shortestPaths = new int[allNodes.Count - 1][];
        for (int indexFirst = 0; indexFirst < allNodes.Count - 1; indexFirst++)
        {
            shortestPaths[indexFirst] = new int[allNodes.Count - indexFirst - 1];
        }

        for (int indexFirst = 0; indexFirst < allNodes.Count - 1; indexFirst++)
        {
            int[] array = shortestPaths[indexFirst];
            for (int indexOffset = 0; indexOffset < array.Length; indexOffset++)
            {
                array[indexOffset] = int.MaxValue;
            }
        }
    }

    // Use (I think this is A*) search to find shortest path between points in the maze, return the step length.
    private int CalculateShortestPath(Point pointOne, Point pointTwo)
    {
        PriorityQueue<DuctMazerNode, int> nodeQueue = new();
        Dictionary<Point, int> visitedSteps = [];
        visitedSteps.Add(pointOne, 0);
        DuctMazerNode startNode = new(pointOne, 0);
        nodeQueue.Enqueue(startNode, 0);
        while (nodeQueue.Count > 0)
        {
            DuctMazerNode node = nodeQueue.Dequeue();

            if (node.Position.Equals(pointTwo))
            {
                return node.StepsTaken;
            }
            else
            {
                foreach (DuctMazerNode neighbour in GetNeighbours(node, pointTwo))
                {
                    // Lookup current position, if we have reached here previously in less or equal steps, prune.
                    if (visitedSteps.TryGetValue(neighbour.Position, out int stepsPrevious) && stepsPrevious <= neighbour.StepsTaken)
                    {
                        continue;
                    }
                    visitedSteps[neighbour.Position] = neighbour.StepsTaken;

                    // Queue in priority order of steps taken + lower bound for remaining path length.
                    int distanceToGoal = Math.Abs(neighbour.Position.X - pointTwo.X) + Math.Abs(neighbour.Position.Y - pointTwo.Y);
                    nodeQueue.Enqueue(neighbour, distanceToGoal + neighbour.StepsTaken);
                }
            }
        }
        return -1;
    }

    // Calculate neighbours, not allowing backtracking.
    private List<DuctMazerNode> GetNeighbours(DuctMazerNode node, Point destination)
    {
        
        List<Point> moves = GetMoves();
        PriorityQueue<Point, int> moveOrder = new();
        List<DuctMazerNode> neighbours = [];
        foreach (Point move in moves)
        {
            // No backtracking.
            if (node.LastMove != default && node.LastMove.X == move.X * -1 && node.LastMove.Y == move.Y * -1)
            {
                continue;
            }
            Point startPoint = new(node.Position.X, node.Position.Y);
            DuctMazerNode startNode = new(startPoint, node.StepsTaken);
            startNode.Move(move);
            // No being inside walls.
            if (!_maze[startNode.Position.X][startNode.Position.Y])
            {
                neighbours.Add(startNode);
            }
        }
        return neighbours;
    }

    private List<Point> GetMoves()
    {
        List<Point> moves = [];
        moves.Add(new(1, 0));
        moves.Add(new(-1, 0));
        moves.Add(new(0, 1));
        moves.Add(new(0, -1));
        return moves;
    }

    // Build up from having traversed only the start point, keeping track of the quickest way to visit each set of nodes and ending at a given location.
    private void CalculateBitmaskShortestPaths()
    {
        InitialiseBitmaskArray();
        List<Point> allNodes = GetAllNodes();

        // Generically:
        // Start nodeshit = 1, start path is the mask 1 with endpoint 0 and pathLength = 0;
        int nodesHit = 1;
        bitmaskShortestPaths[1][0] = 0;

        // Loop until we have hit all nodes.
        while (nodesHit < allNodes.Count)
        {
            // Take every mask that hits nodesHit nodes.
            foreach (int mask in GetNodeBitmasks(nodesHit))
            {
                for (int endPoint = 0; endPoint < allNodes.Count; endPoint++)
                {
                    // Only for the first traversal do we want to allow the original point to be the start point.
                    if (nodesHit != 1 && endPoint == 0)
                    {
                        continue;
                    }
                    int endBit = 1 << endPoint;
                    // If the end point we are checking isnt in the traversed nodes, the path isnt possible, move on.
                    if ((mask & endBit) == 0)
                    {
                        continue;
                    }
                    int originalPathLength = bitmaskShortestPaths[mask][endPoint];
                    for (int newEndPoint = 1; newEndPoint < allNodes.Count; newEndPoint++)
                    {
                        // If the next node we attempt is already traversed, we dont want to repeat, move on.
                        int newBit = 1 << newEndPoint;
                        if ((mask & newBit) != 0)
                        {
                            continue;
                        }

                        int newPathLength = originalPathLength + GetShortestPath(endPoint, newEndPoint);
                        int newMask = mask | newBit;
                        int currentMinPath = bitmaskShortestPaths[newMask][newEndPoint];
                        bitmaskShortestPaths[newMask][newEndPoint] = newPathLength < currentMinPath ? newPathLength : currentMinPath;
                    }
                }
            }
            nodesHit++;
        }
    }

    // Loop over fastest full mask path with each given endpoint, for each of them add the distance back to start, and keep the fastest path of all of these.
    private int GetFastestTraversalAndReturnSteps()
    {
        List<Point> allNodes = GetAllNodes();
        int allNodesHitMask = (1 << allNodes.Count) - 1;
        int shortestTraversal = int.MaxValue;
        for (int endPoint = 1; endPoint < allNodes.Count; endPoint++)
        {
            int pathLength = bitmaskShortestPaths[allNodesHitMask][endPoint] + GetShortestPath(0, endPoint);
            shortestTraversal = pathLength < shortestTraversal ? pathLength : shortestTraversal;
        }
        return shortestTraversal;
    }

    // Loop over endpoints with full mask to find the fastest path that hits all nodes.
    private int GetFastestTraversalSteps()
    {
        List<Point> allNodes = GetAllNodes();
        int allNodesHitMask = (1 << allNodes.Count) - 1;
        int shortestTraversal = int.MaxValue;
        for (int endPoint = 1; endPoint < allNodes.Count; endPoint++)
        {
            int pathLength = bitmaskShortestPaths[allNodesHitMask][endPoint];
            shortestTraversal = pathLength < shortestTraversal ? pathLength : shortestTraversal;
        }
        return shortestTraversal;
    }

    private int GetShortestPath(int pointOne, int pointTwo)
    {
        if (pointOne > pointTwo)
        {
            int temp = pointOne;
            pointOne = pointTwo;
            pointTwo = temp;
        }

        return shortestPaths[pointOne][pointTwo - pointOne - 1];
    }

    private IEnumerable<int> GetNodeBitmasks(int nodesHit)
    {
        uint popCountNeeded = (uint)nodesHit - 1;
        for (int mask = 0; mask < (1 << _destinations.Count); mask ++)
        {
            if (BitOperations.PopCount((uint)mask) == popCountNeeded)
            {
                yield return (mask << 1) | 1;
            }
        }
    }

    private void InitialiseBitmaskArray()
    {
        List<Point> allNodes = GetAllNodes();
        int bitmaskCount = 1 << allNodes.Count;
        bitmaskShortestPaths = new int[bitmaskCount][];
        for (int bitmask = 1; bitmask < bitmaskCount; bitmask++)
        {
            bitmaskShortestPaths[bitmask] = new int[allNodes.Count];
            for (int endIndex = 0; endIndex < allNodes.Count; endIndex++)
            {
                bitmaskShortestPaths[bitmask][endIndex] = int.MaxValue;
            }
        }
    }

    private List<Point> GetAllNodes()
    {
        List<Point> allNodes = [.. _destinations];
        allNodes.Insert(0, _startPosition);
        return allNodes;
    }
}