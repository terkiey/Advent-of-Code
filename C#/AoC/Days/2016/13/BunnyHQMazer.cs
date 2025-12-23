using System.Xml.Linq;

namespace AoC.Days;

internal class BunnyHQMazer
{
    private readonly BunnyMaze _maze;

    public BunnyHQMazer(string seed)
    {
        if (!int.TryParse(seed, out int mazeSeed))
        {
            mazeSeed = 0;
        }
        _maze = new(mazeSeed);
    }

    public int DistinctTilesHit(int steps)
    {
        ExtendMaze(steps + 1, steps + 1);

        StepCounterMazerNode startNode = new([-2, -2], 1, 1, 0);
        Queue<StepCounterMazerNode> nodeQueue = new();
        nodeQueue.Enqueue(startNode);

        HashSet<Position2D> visitedTiles = [];

        while (nodeQueue.Count > 0)
        {
            StepCounterMazerNode currentNode = nodeQueue.Dequeue();
            
            // If in a wall, prune.
            if (_maze.Maze[currentNode.Position.x][currentNode.Position.y])
            {
                continue;
            }

            visitedTiles.Add(currentNode.Position);

            // Generate moves;
            List<StepCounterMazerNode> neighbourNodes = AllMovesStepCounter(currentNode);

            // If already visited that tile, or steps taken is over 50, prune.
            // Add to queue:
            foreach (StepCounterMazerNode neighbourNode in neighbourNodes)
            {
                if (visitedTiles.Contains(neighbourNode.Position) || neighbourNode.Steps > 50)
                {
                    continue;
                }
                nodeQueue.Enqueue(neighbourNode);
            }
        }
        return visitedTiles.Count();
    }

    public int StepsRequiredFor(int xDest, int yDest)
    {
        ExtendMaze(xDest, yDest);

        MazerNode startNode = new([-2, -2], 1, 1);
        Position2D destination = new(xDest, yDest);

        Stack<MazerNode> activeStack= new();
        Stack<MazerNode> frozenStack = new();
        activeStack.Push(startNode);


        HashSet<Position2D> currentVisitedTiles = [];
        HashSet<Position2D> pastVisitedTiles = [];
        int nonTaxiCabMoves = 0;
    
        while (activeStack.Count > 0)
        {
            MazerNode currentNode = activeStack.Pop();
            currentVisitedTiles.Add(currentNode.Position);
            // If you are at the destination, report back the move count.
            if (currentNode.Position.x == xDest && currentNode.Position.y == yDest)
            {
                return (xDest - 1) + (yDest - 1) + (nonTaxiCabMoves * 2);
            }

            // If in a wall, prune.
            if (_maze.Maze[currentNode.Position.x][currentNode.Position.y])
            {
                if (activeStack.Count == 0)
                {
                    nonTaxiCabMoves++;
                    ExtendMaze(xDest + nonTaxiCabMoves, yDest + nonTaxiCabMoves);
                    activeStack = new Stack<MazerNode>(frozenStack);
                    frozenStack.Clear();
                    pastVisitedTiles.UnionWith(currentVisitedTiles);
                }
                continue;
            }

            // Generate moves;
            List<MazerNode> neighbourNodes = AllMoves(currentNode);

            // If visiting a tile visited with less taxicab moves, prune.
            // Assign to relevant stack based on whether its a taxicab move.
            foreach (MazerNode neighbourNode in neighbourNodes)
            {
                if (pastVisitedTiles.Contains(neighbourNode.Position))
                {
                    continue;
                }

                int xDiff = xDest - currentNode.Position.x;
                int yDiff = yDest - currentNode.Position.y;

                if (xDiff > 0 && neighbourNode.LastMove[0] < 0)
                {
                    frozenStack.Push(neighbourNode);
                }
                else if (xDiff < 0 && neighbourNode.LastMove[0] > 0)
                {
                    frozenStack.Push(neighbourNode);
                }
                else if (xDiff == 0 && neighbourNode.LastMove[0] != 0)
                {
                    frozenStack.Push(neighbourNode);
                }
                else if (yDiff > 0 && neighbourNode.LastMove[1] < 0)
                {
                    frozenStack.Push(neighbourNode);
                }
                else if (yDiff < 0 && neighbourNode.LastMove[1] > 0)
                {
                    frozenStack.Push(neighbourNode);
                }
                else if (yDiff == 0 && neighbourNode.LastMove[1] != 0)
                {
                    frozenStack.Push(neighbourNode);
                }
                else
                {
                    activeStack.Push(neighbourNode);
                }
            }

            if (activeStack.Count == 0)
            {
                nonTaxiCabMoves++;
                ExtendMaze(xDest + nonTaxiCabMoves, yDest + nonTaxiCabMoves);
                activeStack = new Stack<MazerNode>(frozenStack);
                frozenStack.Clear();
                pastVisitedTiles.UnionWith(currentVisitedTiles);
            }
        }
        return -1;
    }

    /* Construct the maze:
         * Parse the input up to the taxicab square of the destination.
         * If the pathfinder ever wants to go beyond those bounds, generate another layer of the square lazily.
         *
         * Pathfind through the maze with your algorithm:
         * Prune paths that reach already reached positions with a longer step count.
         * Disallow generating the neighbour that is simply backtracking where you just came from, so track last move on a path node and dont allow the inverse of it to be produced as a neighbour.
         * Keep moves as (0,1), (0, -1), (1,0) and (-1,0) its an exhautsive list, remove the inverse of last move and try the moves in order of priority as described above.
         * 
         * Suggested Algorithm:
         *  1) Generate the active queue for pathnodes. Add (1,1) with no prior move (no need to track steps taken)
         *  2) Generate neighbours. For those that follow taxicab rules toward destination, add to active queue for processing. For those that dont, put in frozen queue.
         *  3) Track visited coordinates for the active queue as you process nodes.
         *  4) Eventually you will reach destination (optimal path is taxicab in length), or you wont.
         *  5) Now, if not done, add one to non-taxicab move counter and make the frozen queue the active queue, and repeat: taxicab towards destination.
         *  6) If you reach any tile that is in the list of tiles from the old queue, prune yourself, as you are a slower method of reaching that tile.
         *  7) All the while, add taxicab nodes to the active processing queue, and non-taxicab to a frozen queue (can just re-use the old one if we swapped them as it will be empty now).
         *  8) If you reach the goal, you are done, the answer is taxicabdistance + 2*non-taxicab moves.
         *  
         *  IF I ADD PATHNODES TO A STACK IT WILL ENSURE LIFO (last in first out) i.e. searching deeply first, for the fastest solution when available at the given path length.
         *  Think of a stack as a LIFO queue.
         *  In fact this will also ensure we process the frozen queue greedily as well, calculating all the deepest and closest to the goal nodes to completion first, before checking really far out stuff.
         *  
         * If failed, take every visited point, make a non-taxicab move, if this results in a visited tile, you could have got to that tile faster using taxicab moves, so prune yourself.
         * otherwise, try taxi-cabbing to destination again for each of these non-taxicab move paths that made unique tiles. If any of these paths ever visit a previously visited tile
         */

    private void ExtendMaze(int x, int y)
    {
        _maze.ExtendTo(x, y);
    }

    private List<MazerNode> AllMoves(MazerNode node)
    {
        List<MazerNode> neighbours = [];
        if (node.Position.x != 0)
        {
            neighbours.Add(node.Move(-1, 0));
        }
        if (node.Position.y != 0)
        {
            neighbours.Add(node.Move(0, -1));
        }
        neighbours.Add(node.Move(1, 0));
        neighbours.Add(node.Move(0, 1));
        
        return neighbours;
    }

    private List<StepCounterMazerNode> AllMovesStepCounter(StepCounterMazerNode node)
    {
        List<StepCounterMazerNode> neighbours = [];
        if (node.Position.x != 0)
        {
            neighbours.Add(node.Move(-1, 0));
        }
        if (node.Position.y != 0)
        {
            neighbours.Add(node.Move(0, -1));
        }
        neighbours.Add(node.Move(1, 0));
        neighbours.Add(node.Move(0, 1));

        return neighbours;
    }
}
