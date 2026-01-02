using System.Xml.Linq;

namespace AoC.Days;

internal class StreamGroup
{
    public int Id { get; }
    public int Score { get; private set; }
    public List<StreamGroup> Groups { get; } = [];
    public List<string> GarbageStrings { get; } = [];

    public StreamGroup(int id)
    {
        Score = 1;
        Id = id;
    }

    public void AddChild(StreamGroup streamGroup)
    {
        Groups.Add(streamGroup);
        streamGroup.UpdateScore(Score + 1);
    }

    public void RemoveChild(StreamGroup streamGroup)
    {
        Groups.Remove(streamGroup);
        streamGroup.UpdateScore(1);
    }

    public void UpdateScore(int score)
    {
        Score = score++;
        foreach (StreamGroup streamGroup in Groups)
        {
            streamGroup.UpdateScore(score);
        }
    }

    public bool Equals(StreamGroup other)
    {
        if (!ReferenceEquals(this, other) || other is null)
        {
            return false;
        }
        return this.Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is StreamGroup other && Equals(other);
    }

    public static bool operator ==(StreamGroup left, StreamGroup right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(StreamGroup left, StreamGroup right)
    {
        return !left.Equals(right);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Id);
        return hash.ToHashCode();
    }
}
