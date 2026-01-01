namespace AoC.Days;

internal class DiscTower
{
    public string Name { get; init; }
    public int Weight { get; init; }
    public List<DiscTower> TowersHeld { get; private set; }
    public int Level { get; private set; }
    
    public DiscTower(string name, int weight)
    {
        Name = name;
        Weight = weight;
        TowersHeld = [];
        Level = 0;
    }

    public void AddHeldTower(DiscTower towerHeld)
    {
        TowersHeld.Add(towerHeld);
        towerHeld.UpdateLevel(Level + 1);
    }

    public void RemoveHeldTower(DiscTower towerHeld)
    {
        TowersHeld.Remove(towerHeld);
        towerHeld.UpdateLevel(0);
    }

    public int GetCombinedWeight()
    {
        int combinedWeight = Weight;
        foreach(DiscTower tower in TowersHeld)
        {
            combinedWeight += tower.GetCombinedWeight();
        }
        return combinedWeight;
    }

    public void UpdateLevel(int level)
    {
        Level = level++;
        foreach(DiscTower tower in TowersHeld)
        {
            tower.UpdateLevel(level);
        }
    }

    public bool Equals(DiscTower other)
    {
        if (!ReferenceEquals(this, other) || other is null)
        {
            return false;
        }
        return this.Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is DiscTower other && Equals(other);
    }

    public static bool operator ==(DiscTower left, DiscTower right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DiscTower left, DiscTower right)
    { 
        return !left.Equals(right); 
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Name);
        return hash.ToHashCode();
    }
}
