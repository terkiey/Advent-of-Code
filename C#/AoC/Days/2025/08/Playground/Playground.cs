namespace AoC.Days;

internal class Playground : IPlayground
{
    // Distances between every pair of boxes in a min-heap
    private PriorityQueue<HashSet<JunctionBox>, double> _pairDistanceHeap = new();
    // All boxes, for reference
    private List<JunctionBox> _allBoxes = [];

    // Current set of circuits
    public List<Circuit> Circuits { get; } = [];
    // Last pair joined
    public List<JunctionBox> LastPair { get; private set; } = [];

    public Playground(string[] lines)
    {
        foreach(string line in lines)
        {
            // Initialise box list, and make a separate circuit for each box.
            string[] coords = line.Split(',');
            JunctionBox box = new(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));
            _allBoxes.Add(box);
            Circuits.Add(new(box));
            
        }
    }

    public void CalculateAllDistances()
    {
        int boxCount = _allBoxes.Count;
        for (int firstBoxIndex = 0; firstBoxIndex < boxCount - 1; firstBoxIndex++)
        {
           JunctionBox firstBox = _allBoxes[firstBoxIndex];
           for (int secondBoxIndex = firstBoxIndex + 1;  secondBoxIndex < boxCount; secondBoxIndex++)
           {
                JunctionBox secondBox = _allBoxes[secondBoxIndex];
                HashSet <JunctionBox> pair = [];
                pair.Add(firstBox);
                pair.Add(secondBox);

                double distance = firstBox.DistanceBetween(secondBox);
                _pairDistanceHeap.Enqueue(pair, distance);
           }
        }
    }

    public void ConnectClosestPair()
    {
        LastPair = _pairDistanceHeap.Dequeue().ToList();
        List<Circuit> associatedCircuits = [];

        foreach(JunctionBox box in LastPair)
        {
            Circuit boxCircuit = Circuits.Where(c => c.Contains(box)).First();
            associatedCircuits.Add(boxCircuit);
        }

        if (associatedCircuits[0] == associatedCircuits[1])
        {
            return;
        }

        Circuit combinedCircuit = associatedCircuits[0].MergeCircuits(associatedCircuits[1], LastPair[0], LastPair[1]);
        Circuits.Remove(associatedCircuits[0]);
        Circuits.Remove(associatedCircuits[1]);
        Circuits.Add(combinedCircuit);
    }
}
