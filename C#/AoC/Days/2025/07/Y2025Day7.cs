namespace AoC.Days;

internal class Y2025Day7 : Day
{
    protected override void RunLogic(string[] lines)
    {
        IBeamSimulator _beamSimulator = new BeamSimulator(lines);
        _beamSimulator.Run();

        AnswerOne = _beamSimulator.BeamSplits.ToString();

        _beamSimulator = new BeamSimulator(lines);
        _beamSimulator.RunQuantum();

        AnswerTwo = _beamSimulator.Timelines.ToString();
    }
}
