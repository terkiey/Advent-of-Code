using System.Security.Cryptography;
using System.Text;

namespace AoC.Days;

internal class VaultMd5Mazer
{
    private string _seed = string.Empty;
    private HashSet<char> openDoorChars = ['b', 'c', 'd', 'e', 'f'];
    public void SetSeed(string seed)
    {
        _seed = seed;
    }

    public string FastestVaultPath()
    {
        // Make a stack of mazer nodes to do a depth first search.
        Stack<VaultMd5MazerNode> nodeStack = new();
        VaultMd5MazerNode startNode = new([]);
        nodeStack.Push(startNode);

        List<char> fastestPath = [];
        while (nodeStack.Count > 0)
        {
            VaultMd5MazerNode currentNode = nodeStack.Pop();
            int[] currentPosition = GetPosition(currentNode);
            // If at vault, update fastest path.
            if (currentPosition.SequenceEqual([4,4]))
            {
                fastestPath = (fastestPath.Count == 0) || (currentNode.PathTaken.Count < fastestPath.Count) ? currentNode.PathTaken : fastestPath;
            }

            // If cannot reach vault faster than ever before, then prune the path.
            if (fastestPath.Count > 0 && currentNode.PathTaken.Count + (4 - currentPosition[0]) + (4- currentPosition[1]) > fastestPath.Count)
            {
                continue;
            }
            
            // Add possible neighbours to the stack for processing.
            foreach (VaultMd5MazerNode neighbour in GetNeighbours(currentNode))
            {
                nodeStack.Push(neighbour);
            }
        }
        return new([.. fastestPath]);
    }

    public string SlowestVaultPath()
    {
        // Make a stack of mazer nodes (going to process all possible paths anyway, queue type doesnt matter)...
        Stack<VaultMd5MazerNode> nodeStack = new();
        VaultMd5MazerNode startNode = new([]);
        nodeStack.Push(startNode);

        List<char> slowestPath = [];
        while (nodeStack.Count > 0)
        {
            VaultMd5MazerNode currentNode = nodeStack.Pop();
            int[] currentPosition = GetPosition(currentNode);
            // If at vault, update slowest path and prune the path afterwards.
            if (currentPosition.SequenceEqual([4, 4]))
            {
                slowestPath = (slowestPath.Count == 0) || (currentNode.PathTaken.Count > slowestPath.Count) ? currentNode.PathTaken : slowestPath;
                continue;
            }

            // Add possible neighbours to the stack for processing.
            foreach (VaultMd5MazerNode neighbour in GetNeighbours(currentNode))
            {
                nodeStack.Push(neighbour);
            }
        }
        return new([.. slowestPath]);
    }

    private List<VaultMd5MazerNode> GetNeighbours(VaultMd5MazerNode currentNode)
    {
        HashSet<char> moves = ['U', 'D', 'L', 'R'];
        int[] currentPosition = GetPosition(currentNode);
        if (currentPosition[0] == 1)
        {
            moves.Remove('L');
        }
        else if (currentPosition[0] == 4)
        {
            moves.Remove('R');
        }

        if (currentPosition[1] == 1)
        {
            moves.Remove('U');
        }
        else if (currentPosition[1] == 4)
        {
            moves.Remove('D');
        }

        HashSet<char> validHashMoves = GetHashMoves(currentNode.PathTaken);
        moves.IntersectWith(validHashMoves);

        List<VaultMd5MazerNode> neighbours = [];
        foreach (char move in moves)
        {
            List<char> path = currentNode.PathTaken.ToList();
            path.Add(move);
            neighbours.Add(new(path));
        }
        return neighbours;
    }

    private HashSet<char> GetHashMoves(List<char> path)
    {
        string hashMe = _seed + new string([.. path]);
        byte[] inputBytes = Encoding.UTF8.GetBytes(hashMe);
        byte[] hashBytes = MD5.HashData(inputBytes);
        string hexString = Convert.ToHexString(hashBytes).ToLower();
        HashSet<char> hashMoves = [];

        if (openDoorChars.Contains(hexString[0]))
        {
            hashMoves.Add('U');
        }
        if (openDoorChars.Contains(hexString[1]))
        {
            hashMoves.Add('D');
        }
        if (openDoorChars.Contains(hexString[2]))
        {
            hashMoves.Add('L');
        }
        if (openDoorChars.Contains(hexString[3]))
        {
            hashMoves.Add('R');
        }
        return hashMoves;
    }

    private int[] GetPosition(VaultMd5MazerNode currentNode)
    {
        int x = 1;
        int y = 1;
        foreach (char character in currentNode.PathTaken)
        {
            switch (character)
            {
                case 'U':
                    y -= 1;
                    break;

                case 'D':
                    y += 1;
                    break;

                case 'L':
                    x -= 1;
                    break;

                case 'R':
                    x += 1;
                    break;
            }
        }
        return [x, y];
    }
}
