using System.Runtime.CompilerServices;

namespace AoC.Days;

internal class BeamSimulator : IBeamSimulator
{
    private List<string> _manifoldLines;
    private int _manifoldWidth;
    private int _manifoldHeight => _manifoldLines.Count;

    private HashSet<int> _beamIndices = [];
    private int stepIndex = 0;

    private Dictionary<int, long> _timelinePositionCounters = [];

    public int BeamSplits { get; private set; }
    public long Timelines => _timelinePositionCounters.Values.Sum();

    public BeamSimulator(string[] lines)
    {
        _manifoldLines = lines.ToList();
        _beamIndices.Add(_manifoldLines[0].IndexOf('S'));
        _manifoldWidth = _manifoldLines[0].Length;

        _timelinePositionCounters.Add(_manifoldLines[0].IndexOf('S'), 1);
    }

    public void Run()
    {
        RemoveBlankLines();
        while (!SimulateStep()) { }
    }

    public void RunQuantum()
    {
        RemoveBlankLines();
        while (!SimulateQuantumStep()) { }
    }

    private void RemoveBlankLines()
    {
        List<string> cleanLines = [];
        foreach (string line in _manifoldLines)
        {
            if (line.Contains("^"))
            {
                cleanLines.Add(line);
            }
        }

        _manifoldLines = cleanLines;
    }

    private bool SimulateStep()
    {
        bool endReached = false;
        foreach(int beamIndex in _beamIndices.ToHashSet())
        {
            if (_manifoldLines[stepIndex][beamIndex] == '^')
            {
                BeamSplits++;
                _beamIndices.Remove(beamIndex);

                if (beamIndex > 0)
                {
                    _beamIndices.Add(beamIndex - 1);
                }

                if (beamIndex < _manifoldWidth - 1)
                {
                    _beamIndices.Add(beamIndex + 1);
                }
                
            }
        }

        stepIndex++;

        if (stepIndex == _manifoldHeight)
        {
            endReached = true;
        }

        return endReached;
    }

    private bool SimulateQuantumStep()
    {
        bool endReached = false;
        foreach (int beamIndex in _timelinePositionCounters.Keys.ToHashSet())
        {
            long timelineCount = _timelinePositionCounters[beamIndex];
            if (_manifoldLines[stepIndex][beamIndex] == '^')
            {
                _timelinePositionCounters.Remove(beamIndex);

                if (beamIndex > 0)
                {
                    TimelineAdd(beamIndex - 1, timelineCount);
                }

                if (beamIndex < _manifoldWidth - 1)
                {
                    TimelineAdd(beamIndex + 1, timelineCount);
                }
            }
        }

        stepIndex++;

        if (stepIndex == _manifoldHeight)
        {
            endReached = true;
        }

        return endReached;
    }

    private void TimelineAdd(int beamIndex, long timelineCountToAdd)
    {
        if(_timelinePositionCounters.TryGetValue(beamIndex, out long timelineCount))
        {
            _timelinePositionCounters[beamIndex] += timelineCountToAdd;
        }
        else
        {
            _timelinePositionCounters.Add(beamIndex, timelineCountToAdd);
        }
    }
}
