namespace AoC.Days;

internal class LightSequenceSolver : ILightSequenceSolver
{
    private bool _tryXButtonsPassed = false;

    public LightSequenceSolver() { }

    public int FewestButtonPresses(MachineManual machineManual)
    {
        int buttonCount = 0;
        while (true)
        {
            if (TryXButtons(buttonCount, machineManual.Buttons, machineManual.Lights)) { break; }
            buttonCount++;
        }

       
        return buttonCount;
    }

    private bool TryXButtons(int buttonCount, int[][] buttonList, bool[] LightSequence)
    {
        TryXButtonsRecursion(buttonCount, buttonList, LightSequence, []);
        if (_tryXButtonsPassed) { _tryXButtonsPassed = false; return true; }
        return false;
    }

    private void TryXButtonsRecursion(int totalButtonsToPick, int[][] buttonList, bool[] LightSequence, List<int> currentButtonIndices)
    {
        // Skip processing if we have already passed the test;
        if (_tryXButtonsPassed) { return; }

        // If we have recursed enough and have the required number of buttons, then check if pressing them gives us the correct light sequence.
        int currentButtonCount = currentButtonIndices.Count();
        if (currentButtonCount == totalButtonsToPick)
        {
            bool[] outputLights = PressButtons(buttonList, currentButtonIndices, LightSequence.Count());
            if (outputLights.SequenceEqual(LightSequence)) { _tryXButtonsPassed = true; }
            return;
        }

        // If we haven't picked enough buttons yet, recursively pick every possible choice (to the right in the list).
        int totalButtonCount = buttonList.Count();
        int remainingButtonsToPick = totalButtonsToPick - currentButtonCount;
        for (int buttonIndex = currentButtonCount; buttonIndex <= totalButtonCount - remainingButtonsToPick; buttonIndex++)
        {
            // Add this loop's button.
            currentButtonIndices.Add(buttonIndex);
            // Recursively create further combinations (or process if combo is full)
            TryXButtonsRecursion(totalButtonsToPick, buttonList, LightSequence, currentButtonIndices);
            // Once this recursion from this point is done, remove the button we added by its predictable index in the list.
            currentButtonIndices.RemoveAt(currentButtonIndices.Count() - 1);
        }
    }

    private bool[] PressButtons(int[][] buttonList, List<int> pressIndices, int lightCount)
    {
        bool[] lightState = new bool[lightCount];
        for (int lightIndex = 0; lightIndex < lightCount; lightIndex++)
        {
            lightState[lightIndex] = false;
        }
        foreach(int pressIndex in pressIndices)
        {
            lightState = PressButton(buttonList[pressIndex], lightState);
        }
        return lightState;
    }

    private bool[] PressButton(int[] button, bool[] preState)
    {
        foreach (int lightIndex in button)
        {
            preState[lightIndex] = !preState[lightIndex];
        }
        return preState;
    }
}
