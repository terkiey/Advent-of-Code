namespace AoC.Days;

internal class Circuit
{
    public Dictionary<JunctionBox, List<JunctionBox>> Connections = [];

    public HashSet<JunctionBox> JunctionBoxes => Connections.Keys.ToHashSet();
    public int CountBoxes => Connections.Count;

    public Circuit(JunctionBox junctionBox)
    {
        Connections.Add(junctionBox, []);
    }

    public Circuit(Dictionary<JunctionBox, List<JunctionBox>> connections)
    {
        Connections = connections;
    }

    public void AddBox(JunctionBox junctionBox)
    {
        JunctionBoxes.Add(junctionBox);
    }

    public bool Contains(JunctionBox junctionBox)
    {
        return JunctionBoxes.Contains(junctionBox);
    }

    public Circuit MergeCircuits(Circuit circuit, JunctionBox boxOne, JunctionBox boxTwo)
    {
        Dictionary<JunctionBox, List<JunctionBox>> newConnections = new(Connections.Union(circuit.Connections));
        newConnections[boxOne].Add(boxTwo);
        newConnections[boxTwo].Add(boxOne);

        return new(newConnections);
    }
}
