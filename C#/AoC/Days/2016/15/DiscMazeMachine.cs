namespace AoC.Days;

internal class DiscMazeMachine
{
    private Disc[] Discs { get; set; }
    public DiscMazeMachine(string[] inputLines)
    {
        Discs = new Disc[inputLines.Length];
        int discIndex = 0;
        foreach(string line in inputLines)
        {
            string[] splitLine = line.Split(' ');
            int positionCount = int.Parse(splitLine[3]);
            int startPosition = int.Parse(splitLine[^1].Replace('.', ' '));
            Discs[discIndex++] = new Disc(startPosition, positionCount);
        }
    }

    public int EarliestPassingDrop()
    {
        int dropIndex = 0;
        while (!TryDrop(dropIndex++)) { }
        return dropIndex - 1;
    }

    public void AddDisc(int startPosition, int positionCount)
    {
        Disc[] newDiscs = new Disc[Discs.Length + 1];
        Discs.CopyTo(newDiscs, 0);
        newDiscs[^1] = new Disc(startPosition, positionCount);
        Discs = newDiscs;
    }

    private bool TryDrop(int dropIndex)
    {
        Disc[] iterDiscs = CloneDiscs();
        foreach (Disc disc in iterDiscs)
        {
            int currentDiscPosition = (++dropIndex + disc.Position) % disc.PositionCount;
            if (currentDiscPosition != 0)
            {
                return false;
            }
        }
        return true;
    }

    private Disc[] CloneDiscs()
    {
        Disc[] output = new Disc[Discs.Length];
        int discIndex = 0;
        foreach (Disc disc in Discs)
        {
            output[discIndex++] = new Disc(disc.Position, disc.PositionCount);
        }
        return output;
    }
}
