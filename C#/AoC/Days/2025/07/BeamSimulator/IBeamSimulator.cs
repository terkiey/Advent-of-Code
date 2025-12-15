namespace AoC.Days;

internal interface IBeamSimulator
{
    int BeamSplits { get; }
    long Timelines { get; }

    void Run();
    void RunQuantum();
}
