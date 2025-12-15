namespace AoC.Days;

internal interface IPlayground
{
    List<Circuit> Circuits { get; }
    List<JunctionBox> LastPair { get; }

    void CalculateAllDistances();
    void ConnectClosestPair();
}
