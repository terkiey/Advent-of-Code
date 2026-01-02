using System.Net.Sockets;

namespace AoC.Days;

internal class FirewallWalker
{
    private Dictionary<int, int[]> FirewallInfo { get; } = []; // 3 ints, range, currentPos and direction (-1 or 1)
    private int walkLength = -1;
    private int Packet { get; set; } = 0;
    public int Severity { get; private set; } = 0;

    private HashSet<int> GetsCaught { get; } = [0];
    private int maxDelayChecked = 0;

    public void InitialisePath(string[] depthInfoArray)
    {
        foreach (string depthInfo in depthInfoArray)
        {
            string[] splitInfo = depthInfo.Split(':');
            int depth = int.Parse(splitInfo[0]);
            int range = int.Parse(splitInfo[1]);
            FirewallInfo[depth] = [range, 0, 1];
            walkLength = depth;
        }
        Packet = 0;
        Severity = 0;
    }

    public bool Walk(int delayStart, bool stopEarly)
    {
        for (int delayIndex = 0; delayIndex < delayStart; delayIndex++)
        {
            FirewallMove();
        }
        while (Packet <= walkLength)
        {
            if (CheckCaught(Packet) && stopEarly)
            {
                return false;
            }
            FirewallMove();
            Packet++;
        }
        return Severity == 0;
    }

    public void ResetState()
    {
        foreach (var kvp in FirewallInfo)
        {
            int[] info = kvp.Value;
            info[1] = 0;
            info[2] = 1;
        }
        Packet = 0;
        Severity = 0;
    }

    // Maybe could make this more efficient by just counting up multiples or something? To avoid dividing... and keep a hashset of invalid values and just compare to that?
    // Would only need to ensure the invalid values is kept up-to-date up to the delay we are checking.
    public bool WillGetCaught(int delay)
    {
        foreach (var kvp in FirewallInfo)
        {
            int depth = kvp.Key;
            int[] info = kvp.Value;
            int range = info[0];
            if ((delay + depth) % ((range - 1) * 2) == 0)
            {
                return true;
            }
        }
        return false;
    }

    // Roughly 2x faster than the first version.
    public bool WillGetCaughtV2(int delay)
    {
        if (delay > maxDelayChecked)
        {
            ExtendCaughtCheckSet(delay + 1000);
        }
        return GetsCaught.Contains(delay);
    }

    private void ExtendCaughtCheckSet(int delay)
    {
        foreach(var kvp in FirewallInfo)
        {
            int depth = kvp.Key;
            int[] info = kvp.Value;
            int range = info[0];
            int multiplicand = (range - 1) * 2;

            int checkAfter = maxDelayChecked - depth;
            int startMultiplier = (checkAfter / multiplicand);
            int maxCheck = -1;
            while (maxCheck < delay)
            {
                int disallow = (multiplicand * startMultiplier++) - depth;
                GetsCaught.Add(disallow);
                maxCheck = disallow;
            }
        }
        maxDelayChecked = delay;
    }

    private bool CheckCaught(int packet)
    {
        if (!FirewallInfo.TryGetValue(packet, out int[]? info))
        {
            return false;
        }

        if (info[1] == 0)
        {
            Severity += packet * info[0];
            return true;
        }
        return false;
    }

    private void FirewallMove()
    {
        foreach (var kvp in FirewallInfo)
        {
            int depth = kvp.Key;
            int[] info = kvp.Value;

            // If the range of the firewall is 1, it never moves.
            if (info[0] == 1)
            {
                continue;
            }

            // Move in current direction.
            info[1] += info[2];

            // If landed at either end of range, swap direction.
            if (info[1] == 0 || info[1] == info[0] - 1)
            {
                info[2] *= -1;
            }
        }
    }
}
