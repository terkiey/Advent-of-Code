using System.Collections.Immutable;

namespace AoC.Days;

internal class DevicePather : IDevicePather
{
    private readonly int _deviceCount;
    private readonly string[][] _outputs;
    private readonly long[] _pathCounts;
    private readonly Dictionary<string, int> _deviceIndices;
    private HashSet<int> _exclude { get; set; }

    public DevicePather(string[] serverRack) 
    {
        _deviceCount = serverRack.Length + 1;
        _deviceIndices = [];
        _outputs = new string[_deviceCount][];
        int[] pathCounts = new int[_deviceCount];
        _exclude = [];
        _pathCounts = new long[_deviceCount];

        for (int deviceIndex = 0; deviceIndex < _deviceCount; deviceIndex++)
        {
            if (deviceIndex == _deviceCount - 1)
            {
                _deviceIndices.Add("out", deviceIndex);
                _outputs[deviceIndex] = [];
                continue;
            }

            _outputs[deviceIndex] = new string[_deviceCount];
            string[] splitRack = serverRack[deviceIndex].Split(':');
            _deviceIndices.Add(splitRack[0], deviceIndex);
            string deviceOutput = splitRack[1].Substring(1);
            _outputs[deviceIndex] = deviceOutput.Split(' ');
        }
    }

    // Constructor for cloning
    private DevicePather(int deviceCount, 
                         string[][] outputs,
                         long[] pathCounts,
                         Dictionary<string, int> deviceIndices,
                         HashSet<int> exclude)
    {
        _deviceCount = deviceCount;
        _outputs = outputs;
        _pathCounts = pathCounts;
        _deviceIndices = deviceIndices;
        _exclude = exclude;
    }

    public IDevicePather Clone()
    {
        var deviceCount = _deviceCount;
        var deviceIndices = new Dictionary<string, int>(_deviceIndices);
        var outputs = _outputs.Select(inner => inner.ToArray()).ToArray();
        var pathCounts = (long[])_pathCounts.Clone();
        var exclude = new HashSet<int>(_exclude);

        return new DevicePather(
           deviceCount,
           outputs,
           pathCounts,
           deviceIndices,
           exclude);
    }

    public long CountPaths(string startDevice, string outDevice)
    {
        return ReverseSearch(startDevice, outDevice);
    }

    public long CountPathsSafe(string startDevice, string outDevice)
    {
        return ReverseSearchSafe(startDevice, outDevice);
    }

    private long ReverseSearchSafe(string startDevice, string outDevice)
    {
        // Reduce the graph connections to the relevant ones, before running ReverseSearch. This can be done by removing outputs for nodes we want excluded.

        HashSet<int> startReachableByNodes = GetReachable(startDevice);
        HashSet<int> outReachedByNodes = GetReached(outDevice, startDevice);
        var tightenedGraphNodes = startReachableByNodes.Intersect(outReachedByNodes);
        _exclude = _deviceIndices.Values.ToHashSet()
                                        .Except(tightenedGraphNodes)
                                        .ToHashSet();
        for (int nodeIndex = 0; nodeIndex < _deviceCount; nodeIndex++)
        {
            if (!tightenedGraphNodes.Contains(nodeIndex))
            {
                _outputs[nodeIndex] = [];
            }
        }

        // Then run ReverseSearch on our tightened graph.
        return ReverseSearch(startDevice, outDevice);
    }

    private HashSet<int> GetReachable(string startDevice)
    {
        // Recursively flow forward until reaching only nodes seen previously or "out".
        Queue<int> queue = [];
        HashSet<int> seen = [];

        int startIndex = _deviceIndices[startDevice];
        seen.Add(startIndex);
        queue.Enqueue(startIndex);

        while (queue.Count > 0)
        {
            int nodeIndex = queue.Dequeue();
            var neighbours = _outputs[nodeIndex].AsEnumerable();
            foreach (var neighbour in neighbours)
            {
                int neighbourIndex = _deviceIndices[neighbour];
                if (neighbour == "out") { seen.Add(neighbourIndex); }
                if(!seen.Add(neighbourIndex))
                {
                    continue;
                }
                queue.Enqueue(neighbourIndex);
            }
        }
        return seen;
    }

    private HashSet<int> GetReached(string outDevice, string startDevice)
    {
        int startDeviceIndex = _deviceIndices[startDevice];
        // Recursively flow backward until reaching only nodes seen previously or the start device.
        Queue<int> queue = [];
        HashSet<int> seen = [];

        int startIndex = _deviceIndices[outDevice];
        seen.Add(startIndex);
        queue.Enqueue(startIndex);

        while (queue.Count > 0)
        {
            int nodeIndex = queue.Dequeue();
            var neighbourIndices = GetInflows(nodeIndex);
            foreach (var neighbourIndex in neighbourIndices)
            {
                if (neighbourIndex == startDeviceIndex)
                {
                    seen.Add(neighbourIndex);
                }
                if (!seen.Add(neighbourIndex))
                {
                    continue;
                }
                queue.Enqueue(neighbourIndex);
            }
        }
        return seen;
    }

    private long ReverseSearch(string startDevice, string outDevice)
    {
        // For iterating.
        Queue<int> queue = [];
        HashSet<int> processed = [];

        // Start with end node in queue and end node starts with pathCount = 1.
        int endIndex = _deviceIndices[outDevice];
        _pathCounts[endIndex] = 1;
        queue.Enqueue(endIndex);

        while (queue.Count > 0)
        {
            int nodeIndex = queue.Dequeue();
            processed.Add(nodeIndex);

            var inflows = GetInflows(nodeIndex);
            foreach (var inflowIndex in inflows)
            {
                if (_exclude.Contains(inflowIndex)) { continue; }
                _pathCounts[inflowIndex] += _pathCounts[nodeIndex];
                bool allOutputsProcessed = true;
                foreach (string outputDevice in _outputs[inflowIndex])
                {
                    int outputIndex = _deviceIndices[outputDevice];
                    if (_exclude.Contains(outputIndex)) { continue; }
                    if (!processed.Contains(outputIndex))
                    {
                        allOutputsProcessed = false;
                        break;
                    }
                }

                if (allOutputsProcessed)
                {
                    queue.Enqueue(inflowIndex);
                }
            }
        }

        int startIndex = _deviceIndices[startDevice];
        return _pathCounts[startIndex];
    }

    private long BreadthFirstSearch(string startDevice)
    {
        ExcludePathsToStart(startDevice);

        // For BFS iterating.
        Queue<int> deviceQueue = [];
        HashSet<int> devicesCalculated = [];

        // Process start node manually.
        int startIndex = _deviceIndices[startDevice];
        _pathCounts[startIndex] = 1;
        devicesCalculated.Add(startIndex);
        string[] startNeighbours = _outputs[startIndex];
        foreach (string neighbour in startNeighbours)
        {
            deviceQueue.Enqueue(_deviceIndices[neighbour]);
        }

        while (deviceQueue.Count > 0)
        {
            // Grab node from queue.
            int nodeIndex = deviceQueue.Dequeue();

            // If any of your sources haven't been calculated, queue them up, add yourself to the queue after them, and then go to next loop.
            long nodeSum = 0;
            bool anyInflowUncalculated = false;
            foreach (int deviceIndex in GetInflows(nodeIndex))
            {
                if (_exclude.Contains(deviceIndex))
                {
                    continue;
                }

                if (devicesCalculated.Contains(deviceIndex))
                {
                    nodeSum += _pathCounts[deviceIndex];
                    continue;
                }
                else
                {
                    deviceQueue.Enqueue(deviceIndex);
                    anyInflowUncalculated = true;
                }
            }

            if (anyInflowUncalculated)
            {
                deviceQueue.Enqueue(nodeIndex);
                continue;
            }

            // Otherwise, complete the node and add its neighbours to the queue.
            _pathCounts[nodeIndex] = nodeSum;
            devicesCalculated.Add(nodeIndex);
            string[] neighbours = _outputs[nodeIndex];
            foreach (string neighbour in neighbours)
            {
                deviceQueue.Enqueue(_deviceIndices[neighbour]);
            }
        }

        int endIndex = _deviceIndices["out"];
        return _pathCounts[endIndex];
    }

    private void ExcludePathsToStart(string startDevice)
    {
        int startIndex = _deviceIndices[startDevice];
        for (int deviceIndex = 0; deviceIndex < _deviceCount; deviceIndex++)
        {
            if (_outputs[deviceIndex].Contains(startDevice))
            {
                _exclude.Add(deviceIndex);
            }
        }
       
        bool loopAgain = true;
        while (loopAgain)
        {
            loopAgain = false;
            foreach (int deviceIndex in _exclude.ToImmutableArray())
            {
                var inflows = GetInflows(deviceIndex);
                foreach (int inflowDevice in inflows)
                {
                    if (_exclude.Add(inflowDevice))
                    {
                        loopAgain = true;
                    }
                }
            }
        }
    }

    private IEnumerable<int> GetInflows(int deviceIndex)
    {
        for (int otherDeviceIndex = 0; otherDeviceIndex < _deviceCount - 1; ++otherDeviceIndex)
        {
            if (deviceIndex == otherDeviceIndex) { continue; }
            string[] candidateOutputs = _outputs[otherDeviceIndex];
            IEnumerable<int> candidateOutputIndices = candidateOutputs.Select((device) => _deviceIndices[device]);
            foreach (int outputIndex in candidateOutputIndices)
            {
                if (outputIndex == deviceIndex)
                {
                    yield return otherDeviceIndex;
                    break;
                }
            }
            
        }
    }
}
