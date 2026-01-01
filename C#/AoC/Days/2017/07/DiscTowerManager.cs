using System.Text.RegularExpressions;

namespace AoC.Days;

internal class DiscTowerManager
{
    private HashSet<string> NamesInitialised { get; } = [];
    private Queue<string> LineQueue { get; } = [];

    public HashSet<DiscTower> TowerSet { get; } = [];
    public List<DiscTower> Towers { get; } = [];

    public void ConstructTowers(string[] inputLines)
    {
        foreach (string programString in inputLines.ToArray())
        {
            TryProcessLine(programString);
        }

        while (LineQueue.Count > 0)
        {
            string line = LineQueue.Dequeue();
            TryProcessLine(line);
        }
    }

    public string BaseTower()
    {
        return Towers.Find(t => t.Level == 0)!.Name;
    }

    public int FixedWeightToBalance()
    {
        foreach (DiscTower tower in Towers)
        {
            if (tower.TowersHeld.Count < 2)
            {
                continue;
            }

            Dictionary<int, int> weightCounts = [];
            Dictionary<int, int> CombinedToSingleWeight = [];
            foreach (DiscTower heldTower in tower.TowersHeld)
            {
                int combinedWeight = heldTower.GetCombinedWeight();
                if (!weightCounts.TryAdd(combinedWeight, 1))
                {
                    weightCounts[combinedWeight]++;
                }

                CombinedToSingleWeight[combinedWeight] = heldTower.Weight;
            }

            if (weightCounts.Count > 1)
            {
                int balanceWeight = weightCounts.Single(kv => kv.Value > 1).Key;
                int badWeight = weightCounts.Single(kv => kv.Value == 1).Key;
                int offset = balanceWeight - badWeight;
                return CombinedToSingleWeight[badWeight] + offset;
            }
        }
        return -1;
    }

    private void TryProcessLine(string line)
    {
        string[] splitLine = line.Split(' ');
        List<DiscTower> heldTowers = [];
        if (splitLine.Length > 3)
        {
            bool heldTowersInitialised = true;
            MatchCollection towerNameMatches = Regex.Matches(line, @"([a-z]+)");
            for (int matchIndex = 1; matchIndex < towerNameMatches.Count; matchIndex++)
            {
                string towerName = towerNameMatches[matchIndex].Value;
                if (!NamesInitialised.Contains(towerName))
                {
                    heldTowersInitialised = false;
                    break;
                }

                heldTowers.Add(Towers.Find(t => t.Name == towerName)!);
            }

            if (!heldTowersInitialised)
            {
                LineQueue.Enqueue(line);
                return;
            }
        }

        string name = splitLine[0];
        int weight = int.Parse(splitLine[1][1..^1]);
        DiscTower tower = new(name, weight);
        if (TowerSet.Add(tower))
        {
            NamesInitialised.Add(name);
            Towers.Add(tower);
        }

        foreach (DiscTower heldTower in heldTowers)
        {
            tower.AddHeldTower(heldTower);
        }
    }
}
