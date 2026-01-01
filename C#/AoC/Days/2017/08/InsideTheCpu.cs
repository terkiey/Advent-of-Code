namespace AoC.Days;

internal class InsideTheCpu
{
    private Dictionary<string, int> Registers { get; set; } = [];
    private Dictionary<string, Func<int, int, bool>> Comparators = new()
    {
        [">"] = (a, b) => a > b,
        ["<"] = (a, b) => a < b,
        [">="] = (a, b) => a >= b,
        ["=="] = (a, b) => a == b,
        ["<="] = (a, b) => a <= b,
        ["!="] = (a, b) => a != b,
    };

    public int highestRegisterEver { get; private set; } = int.MinValue;

    public void ProcessInstructions(string[] instructions)
    {
        foreach (string instruction in instructions)
        {
            string[] splitInstruction = instruction.Split(" if ");
            if(IsConditionMet(splitInstruction[1]))
            {
                ProcessCommand(splitInstruction[0]);
            }
        }
    }

    public int MaxRegValue()
    {
        int maxVal = int.MinValue;
        foreach (var kvp in Registers)
        {
            maxVal = kvp.Value > maxVal ? kvp.Value : maxVal;
        }
        return maxVal;
    }

    private void ProcessCommand(string command)
    {
        string[] splitCommand = command.Split(' ');
        string regName = splitCommand[0];
        int valueTwo = ParseValue(splitCommand[2]);
        _ = GetRegValue(regName);
        switch (splitCommand[1])
        {
            case "inc":
                Registers[regName] += valueTwo;
                break;

            case "dec":
                Registers[regName] -= valueTwo;
                break;
        }
        highestRegisterEver = Registers[regName] > highestRegisterEver ? Registers[regName] : highestRegisterEver;
    }

    private bool IsConditionMet(string condition)
    {
        string[] splitCondition = condition.Split(' ');
        int valueOne = ParseValue(splitCondition[0]);
        int valueTwo = ParseValue(splitCondition[2]);
        return Comparators[splitCondition[1]](valueOne, valueTwo);
    }

    private int ParseValue(string valueString)
    {
        if (int.TryParse(valueString, out var value))
        {
            return value;
        }
        return GetRegValue(valueString);
    }

    private int GetRegValue(string regName)
    {
        if (Registers.TryGetValue(regName, out int regValue))
        {
            return regValue;
        }
        return Registers[regName] = 0;
    }
}
