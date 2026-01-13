namespace AoC.Days;

internal class MagneticComponentBridgeBuilder
{
    private HashSet<int[]> Components { get; } = [];
    public void ParseComponents(string[] componentStrings)
    {
        foreach (string componentString in componentStrings)
        {
            int[] component = new int[2];
            component[0] = int.Parse(componentString.Split('/')[0]);
            component[1] = int.Parse(componentString.Split('/')[1]);
            Components.Add(component);
        }
    }

    // To start, brute force using a breadth-first search. That was actually sufficient!
    public int FindStrongestBridgeStrength()
    {
        Queue<MagneticComponentBridgeNode> nodeQueue = [];
        MagneticComponentBridgeNode starterNode = new();
        nodeQueue.Enqueue(starterNode);
        int maxStrength = int.MinValue;
        while(nodeQueue.Count > 0)
        {
            MagneticComponentBridgeNode node = nodeQueue.Dequeue();
            int strength = node.GetBridgeStrength();
            maxStrength = strength > maxStrength? strength : maxStrength;
            foreach (var neighbour in node.GetPossibilities(Components))
            {
                nodeQueue.Enqueue(neighbour);
            }
        }
        return maxStrength;
    }

    public int FindLongestStrongestBridgeStrength()
    {
        Queue<MagneticComponentBridgeNode> nodeQueue = [];
        MagneticComponentBridgeNode starterNode = new();
        nodeQueue.Enqueue(starterNode);
        int maxStrength = int.MinValue;
        int maxLength = int.MinValue;
        while (nodeQueue.Count > 0)
        {
            MagneticComponentBridgeNode node = nodeQueue.Dequeue();
            int length = node.GetBridgeLength();
            int strength = node.GetBridgeStrength();
            if (length > maxLength)
            {
                maxLength = length;
                maxStrength = int.MinValue;
            }
            if (length >= maxLength)
            {
                maxStrength = strength > maxStrength ? strength : maxStrength;

            }

            foreach (var neighbour in node.GetPossibilities(Components))
                {
                    nodeQueue.Enqueue(neighbour);
                }
        }
        return maxStrength;
    }
}
