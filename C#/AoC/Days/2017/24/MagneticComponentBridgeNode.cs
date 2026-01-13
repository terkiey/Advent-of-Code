namespace AoC.Days;

internal class MagneticComponentBridgeNode
{
    public HashSet<int[]> Components = [];
    public int ExposedPort = 0;

    public MagneticComponentBridgeNode() { }
    public MagneticComponentBridgeNode(HashSet<int[]> components, int exposedPort)
    {
        Components = components;
        ExposedPort = exposedPort;
    }

    public MagneticComponentBridgeNode(MagneticComponentBridgeNode node, int[] newComponent)
    {
        Components = [.. node.Components];
        Components.Add(newComponent);
        if (newComponent[0] == node.ExposedPort)
        {
            ExposedPort = newComponent[1];
        }
        else if (newComponent[1] == node.ExposedPort)
        {
            ExposedPort = newComponent[0];
        }
        else
        {
            throw new ArgumentException("Suggested component doesnt fit!");
        }
    }

    public int GetBridgeStrength()
    {
        int strength = 0;
        foreach(int[] component in Components)
        {
            strength += component[0] + component[1];
        }
        return strength;
    }

    public IEnumerable<MagneticComponentBridgeNode> GetPossibilities(HashSet<int[]> componentList)
    {
        foreach (int[] component in componentList)
        {
            if (Components.Contains(component))
            {
                continue;
            }

            if (component[0] == ExposedPort || component[1] == ExposedPort)
            {
                yield return new(this, component);
            }
        }
    }

    public int GetBridgeLength()
    {
        return Components.Count;
    }
}
