namespace AoC.Days;

/* BFS Implementation, however it is not intelligently spotting impossible branches early.
 * For example, there could be logic that identifies 'linked' joltages that must be increased together, and use that to significantly reduce the number of branches calculated.
 */
internal class JoltageSequenceSolver : IJoltageSequenceSolver
{
    public JoltageSequenceSolver() { }

    public int FewestButtonPresses(MachineManual machineManual)
    {
        Queue<JoltageState> processQueue = [];
        HashSet<JoltageState> statesVisited = [];

        int[][] buttons = machineManual.Buttons;
        int buttonCount = buttons.Count();
        int joltageCount = machineManual.Joltages.Count();
        JoltageState startingState = new JoltageState(new int[buttonCount]);

        processQueue.Enqueue(startingState);
        statesVisited.Add(startingState);

        while (processQueue.Count > 0)
        { 
            JoltageState state = processQueue.Dequeue();
            int[] stateJoltages = state.ComputeJoltages(buttons, joltageCount);

            if (StateIsSolution(stateJoltages, machineManual.Joltages, joltageCount))
            {
                return state.ButtonPressCounts.Sum();
            }

            foreach (JoltageState neighbour in state.CreateNeighbours(buttons))
            {
                if (Overshoots(stateJoltages, machineManual.Joltages)) { continue; }

                if (statesVisited.Add(neighbour))
                { processQueue.Enqueue(neighbour); } 
            }
        }

        return int.MinValue;
    }

    private bool StateIsSolution(int[] stateJoltages, int[] desiredJoltages, int joltageCount)
    {
        for (int joltageIndex = 0; joltageIndex < joltageCount; joltageIndex++)
        {
            if (stateJoltages[joltageIndex] != desiredJoltages[joltageIndex])
            {
                return false;
            }
        }
        return true;
    }

    private bool Overshoots(int[] stateJoltages, int[] desiredJoltages)
    {
        for (int joltageIndex = 0; joltageIndex < stateJoltages.Length; joltageIndex++)
        {
            if (stateJoltages[joltageIndex] > desiredJoltages[joltageIndex])
            {
                return true;
            }
        }
        return false;
    }
}
