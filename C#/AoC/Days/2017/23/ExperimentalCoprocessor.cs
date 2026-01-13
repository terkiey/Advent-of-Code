namespace AoC.Days;

internal class ExperimentalCoprocessor
{
    // CPU Stuff
    public Dictionary<char, long> Registers { get; } = [];
    public long Pointer { get; set; } = 0;
    private const int RegisterCount = 8;

    // Instructions
    private List<Action<ExperimentalCoprocessor>> InstructionList { get; } = [];

    // AOC
    public long MulInvocationCounter = 0;

    public void RunProgram(string[] program)
    {
        ParseInstructions(program);

        while (Pointer >= 0 && Pointer < InstructionList.Count)
        {
            InstructionList[(int)Pointer](this);
        }
    }

    public void InitialiseComputerState()
    {
        Registers.Clear();
        Pointer = 0;
        InstructionList.Clear();
        MulInvocationCounter = 0;

        for (int registerIndex = 0; registerIndex < RegisterCount;  registerIndex++)
        {
            char registerName = (char)('a' + registerIndex);
            Registers[registerName] = 0;
        }
    }

    private void ParseInstructions(string[] program)
    {
        foreach (string instruction in program)
        {
            ParseInstruction(instruction);
        }
    }

    private void ParseInstruction(string instruction)
    {
        string type = instruction[..3];
        string arg1 = instruction.Split(' ')[1];
        string arg2 = instruction.Split(' ')[2];
        Action<ExperimentalCoprocessor> action;
        
        switch (type)
        {
            case "set":
                action = cpu => cpu.Set(arg1[0], arg2);
                break;

            case "sub":
                action = cpu => cpu.Sub(arg1[0], arg2);
                break;

            case "mul":
                action = cpu => cpu.Mul(arg1[0], arg2);
                break;

            case "jnz":
                action = cpu => cpu.Jnz(arg1, arg2);
                break;

            default:
                throw new ArgumentException("Unidentified command passed");
        }
        InstructionList.Add(action);
    }

    private void Set(char registerName, string arg2)
    {
        Registers[registerName] = ParseArg(arg2);
        Pointer++;
    }

    private void Sub(char registerName, string arg2)
    {
        Registers[registerName] -= ParseArg(arg2);
        Pointer++;
    }

    private void Mul(char registerName, string arg2)
    {
        Registers[registerName] *= ParseArg(arg2);
        MulInvocationCounter++;
        Pointer++;
    }

    private void Jnz(string arg1, string arg2)
    {
        if (ParseArg(arg1) != 0)
        {
            Pointer += ParseArg(arg2);
        }
        else
        {
            Pointer++;
        }
    }

    private long ParseArg(string arg)
    {
        if(!long.TryParse(arg, out long value))
        {
            return Registers[arg[0]];
        }
        return value;
    }
}
