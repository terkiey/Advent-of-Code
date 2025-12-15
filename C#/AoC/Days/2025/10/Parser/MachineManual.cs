namespace AoC.Days;

internal class MachineManual
{
    public bool[] Lights {get; }
    public int[][] Buttons { get; }
    public int[] Joltages { get; }

    public MachineManual(bool[] lights, int[][] buttons, int[] joltages)
    {
        Lights = lights;
        Buttons = buttons;
        Joltages = joltages;
    }
}
