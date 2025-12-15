using System.Collections.Concurrent;

namespace AoC.Days;

internal class Y2025Day10: Day
{
    protected override void RunLogic(string[] lines)
    {
        IMachineManualParser manualParser = new MachineManualParser();
        ILightSequenceSolver lightSolver = new LightSequenceSolver();
        IJoltageSequenceSolver joltageSolver = new JoltageSequenceSolverLP();

        MachineManual[] machineManuals = manualParser.Parse(lines);

        int AnswerOneNum = 0;
        
        foreach (MachineManual machineManual in machineManuals)
        {
            AnswerOneNum += lightSolver.FewestButtonPresses(machineManual);
            
        }
        AnswerOne = AnswerOneNum.ToString();

        var partitioner = Partitioner.Create(machineManuals, EnumerablePartitionerOptions.NoBuffering);
        int AnswerTwoNum = 0;
 
        Parallel.ForEach(partitioner,
            machineManual =>
        {
            int joltageSolve = joltageSolver.FewestButtonPresses(machineManual);
            
            Interlocked.Add(ref AnswerTwoNum, joltageSolve);
        });
        AnswerTwo = AnswerTwoNum.ToString();
    }
}
