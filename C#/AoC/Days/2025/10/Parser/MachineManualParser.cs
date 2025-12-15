using System.Text.RegularExpressions;

namespace AoC.Days;

internal class MachineManualParser : IMachineManualParser
{
    public MachineManualParser() { }

    public MachineManual[] Parse(string[] inputManualStrings)
    {
        MachineManual[] outputManuals = new MachineManual[inputManualStrings.Length];
        for (int manualIndex = 0; manualIndex < inputManualStrings.Length; manualIndex++)
        {
            string manualString = inputManualStrings[manualIndex];
            int LightStartIndex = manualString.IndexOf('[');
            int LightEndIndex = manualString.IndexOf("]");
            int ButtonsStartIndex = manualString.IndexOf('(');
            int ButtonsEndIndex = manualString.LastIndexOf(')');
            int JoltagesStartIndex = manualString.IndexOf("{");
            int JoltagesEndIndex = manualString.IndexOf("}");

            string Lights = manualString.Substring(LightStartIndex + 1, (LightEndIndex - LightStartIndex) - 1);
            string Buttons = manualString.Substring(ButtonsStartIndex, (ButtonsEndIndex - ButtonsStartIndex) + 1);
            string Joltages = manualString.Substring(JoltagesStartIndex + 1, (JoltagesEndIndex - JoltagesStartIndex) - 1);

            bool[] manualLights = ParseLights(Lights);
            int[][] manualButtons = ParseButtons(Buttons);
            int[] manualJoltages = ParseJoltages(Joltages);

            outputManuals[manualIndex] = new(manualLights, manualButtons, manualJoltages);
        }
        return outputManuals;
    }

    private bool[] ParseLights(string lightsString)
    {
        bool[] bools = new bool[lightsString.Length];
        for (int charIndex = 0; charIndex < lightsString.Length; charIndex++)
        {
            char light = lightsString[charIndex];
            switch (light)
            {
                case '#':
                    bools[charIndex] = true;
                    break;
            }
        }
        return bools;
    }

    private int[][] ParseButtons(string buttonsString)
    {
        string[] buttonStrings = buttonsString.Split(' ');
        int[][] buttons = new int[buttonStrings.Length][];
        for (int buttonIndex = 0; buttonIndex < buttonStrings.Length; buttonIndex++)
        {
            string buttonString = buttonStrings[buttonIndex];
            int[] button = ParseButton(buttonString);
            buttons[buttonIndex] = button;
        }
        return buttons;
    }

    private int[] ParseJoltages(string joltagesString)
    {
        string[] joltageStrings = joltagesString.Split(',');
        int[] joltages = new int[joltageStrings.Length];
        for (int joltageIndex = 0; joltageIndex < joltageStrings.Length; joltageIndex++)
        {
            string joltageString = joltageStrings[joltageIndex];
            joltages[joltageIndex]=int.Parse(joltageString);
        }
        return joltages;
    }

    private int[] ParseButton(string buttonString)
    {
        string cleanButton = buttonString.Substring(1, buttonString.Length - 2);
        string[] LightIndices = cleanButton.Split(',');
        int[] button = new int[LightIndices.Length];
        for (int LightIndexIndex = 0; LightIndexIndex < LightIndices.Length; LightIndexIndex++)
        {
            string LightIndex = LightIndices[LightIndexIndex];
            button[LightIndexIndex] = int.Parse(LightIndex);
        }
        return button;
    }
}
