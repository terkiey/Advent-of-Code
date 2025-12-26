namespace AoC.Days;

internal class ElfElephantPartySimulator
{
    public int LastElfLinear(string elfCountString)
    {
        int elfCount = int.Parse(elfCountString);
        Queue<int> turnOrder = new();
        bool[] circle = new bool[elfCount];
        for(int elfIndex = 0; elfIndex < elfCount; elfIndex++)
        {
            turnOrder.Enqueue(elfIndex);
            circle[elfIndex] = true;
        }

        while(turnOrder.Count > 1)
        {
            int elfIndex = turnOrder.Dequeue();
            if (!circle[elfIndex])
            {
                continue;
            }

            int nextElfIndex = NextElfLinear(circle, elfIndex);
            circle[nextElfIndex] = false;
            turnOrder.Enqueue(elfIndex);
        }

        return turnOrder.Dequeue() + 1;
    }

    public int LastElfAcross(string elfCountString)
    {
        int startElfCount = int.Parse(elfCountString);
        int currentCount = 5;
        int winner = 2;
        while (currentCount < startElfCount)
        {
            winner += winner >= (currentCount + 1) / 2 ? 2 : 1;
            if (winner > ++currentCount)
            {
                winner = 1;
            }
        }
        return winner;
    }

    public int LastElfAcrossBrute(string elfCountString)
    {
        int startElfCount = int.Parse(elfCountString);
        Queue<ElephantPartyElfNode> turnOrder = new();
        List<ElephantPartyElfNode> circle = [];
        for (int elfNumber = 1; elfNumber <= startElfCount; elfNumber++)
        {
            ElephantPartyElfNode newElf = new(elfNumber);
            turnOrder.Enqueue(newElf);
            circle.Add(newElf);
        }

        while(turnOrder.Count > 1)
        {
            ElephantPartyElfNode elf = turnOrder.Dequeue();
            if (!elf.InGame)
            {
                continue;
            }

            int elfIndex = circle.IndexOf(elf);
            ElephantPartyElfNode targetElf = NextElfAcross(circle, elfIndex);
            circle.Remove(targetElf);
            targetElf.InGame = false;
            turnOrder.Enqueue(elf);
        }
        return turnOrder.Dequeue().ElfNumber;
    }

    private int NextElfLinear(bool[] circle, int elfIndex)
    {
        for(int indexOffset = 1; indexOffset < circle.Length; indexOffset++)
        {
            int elfCheckIndex = (elfIndex + indexOffset) % circle.Length;
            if (circle[elfCheckIndex])
            {
                return elfCheckIndex;
            }
        }
        return -1;
    }

    private ElephantPartyElfNode NextElfAcross(List<ElephantPartyElfNode> circle, int elfIndex)
    {
        int nextElfIndex = (elfIndex + (circle.Count / 2)) % circle.Count;
        return circle[nextElfIndex];
    }
}
