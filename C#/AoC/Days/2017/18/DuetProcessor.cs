namespace AoC.Days;

internal class DuetProcessor
{
    // CPU Stuff
    public Dictionary<char, long> Registers { get; } = [];
    public int Pointer { get; set; } = 0;

    // Instructions
    private List<Action<DuetProcessor>> InstructionList { get; } = [];

    // Send/Sound & Receive/Recover events
    public event EventHandler<char>? Recover;
    public event EventHandler<long>? Sound;

    // Request Indicators
    bool SoundPlayed = false;
    bool RecoverRequested = false;

    public void RunUntilSend()
    {
        SoundPlayed = false;
        while (!SoundPlayed && Pointer >= 0 && Pointer < InstructionList.Count)
        {
            InstructionList[Pointer++](this);
        }
    }

    public bool RunUntilRecover()
    {
        RecoverRequested = false;
        while (!RecoverRequested && Pointer >= 0 && Pointer < InstructionList.Count)
        {
            InstructionList[Pointer++](this);
        }
        if (RecoverRequested)
        {
            return true;
        }
        return false;
    }

    public void ResetState()
    {
        foreach (var kvp in Registers)
        {
            Registers[kvp.Key] = 0;
            Pointer = 0;
        }
    }

    public void ParseInstructions(string[] instructions)
    {
        InstructionList.Clear();
        foreach (string instruction in instructions)
        {
            string[] split = instruction.Split(' ');
            string commandType = split[0];
            switch (commandType)
            {
                case "snd":
                    InstructionList.Add(d => d.PlaySound(split[1]));
                    break;

                case "set":
                    Registers[split[1][0]] = 0;
                    InstructionList.Add(d => d.SetRegister(split[1][0], split[2]));
                    break;

                case "add":
                    Registers[split[1][0]] = 0;
                    InstructionList.Add(d => d.AddToRegister(split[1][0], split[2]));
                    break;

                case "mul":
                    Registers[split[1][0]] = 0;
                    InstructionList.Add(d => d.MultiplyRegister(split[1][0], split[2]));
                    break;

                case "mod":
                    Registers[split[1][0]] = 0;
                    InstructionList.Add(d => d.ModuloRegister(split[1][0], split[2]));
                    break;

                case "rcv":
                    InstructionList.Add(d => d.RecoverFrequency(split[1][0]));
                    break;

                case "jgz":
                    InstructionList.Add(d => d.JumpGreaterThanZero(split[1], split[2]));
                    break;
            }
        }
    }

    private void PlaySound(string arg1)
    {
        Sound?.Invoke(this, ParseArg(arg1));
        SoundPlayed = true;
    }

    private void SetRegister(char register, string arg2)
    {
        Registers[register] = ParseArg(arg2);
    }

    private void AddToRegister(char register, string arg2)
    {
        Registers[register] += ParseArg(arg2);
    }

    private void MultiplyRegister(char register, string arg2)
    {
        Registers[register] *= ParseArg(arg2);
    }

    private void ModuloRegister(char register, string arg2)
    {
        Registers[register] %= ParseArg(arg2);
    }

    private void RecoverFrequency(char arg1)
    {
        Recover?.Invoke(this, arg1);
        RecoverRequested = true;
    }

    // Jump by one less, because we iterate the pointer up for each command that is run.
    private void JumpGreaterThanZero(string arg1, string arg2)
    {
        if (ParseArg(arg1) <= 0)
        {
            return;
        }
        else
        {
            Pointer += (int)ParseArg(arg2) - 1;
        }
    }

    private long ParseArg(string arg)
    {
        long output;
        if (long.TryParse(arg, out output))
        {
            return output;
        }

        if (!Registers.TryGetValue(arg[0], out long value))
        {
            return Registers[arg[0]] = 0;
        }
        return value;
    }
}
