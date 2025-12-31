namespace AoC.Days;

// I have paid dearly for my crime of not extending the original assembunny computer, welp too far gone now, so just copy paste all of it again!
internal class AssembunnyComputerV3
{
    private long _pointer = 0;
    private List<string> _regNames = ["a", "b", "c", "d"];
    public Dictionary<string, long> Registers = new()
    {
        { "a", 0 },
        { "b", 0 },
        { "c", 0 },
        { "d", 0 },
    };

    public event EventHandler<long>? ClockSignal;

    public void ProcessInstructions(string[] instructions)
    {
        _pointer = 0;

        while (_pointer < instructions.Length && _pointer >= 0)
        {
            string instruction = instructions[_pointer];
            string[] splitInstruction = instruction.Split(' ');
            switch (splitInstruction[0])
            {
                case "cpy":
                    string copyValue = splitInstruction[1];
                    string target = splitInstruction[2];
                    ProcessCopy(copyValue, target);
                    break;

                case "inc":
                    target = splitInstruction[1];
                    RegisterAdd(target, 1);
                    break;

                case "dec":
                    target = splitInstruction[1];
                    RegisterAdd(target, -1);
                    break;

                case "jnz":
                    string check = splitInstruction[1];
                    string jumpSize = splitInstruction[2];
                    ProcessJnz(check, jumpSize);
                    break;

                case "out":
                    ProcessOut(splitInstruction[1]);
                    break;
            }
        }
    }

    public void ClearRegisters()
    {
        foreach (string regName in _regNames)
        {
            Registers[regName] = 0;
        }
    }

    private long ParseValue(string valueString)
    {
        if (!long.TryParse(valueString, out long value))
        {
            return Registers[valueString];
        }
        return value;
    }

    private void ProcessJnz(string check, string jumpSizeString)
    {
        long checkValue = ParseValue(check);
        long jumpValue = checkValue == 0 ? 1 : ParseValue(jumpSizeString);
        _pointer += jumpValue;
    }

    private void RegisterAdd(string target, long addValue)
    {
        if (long.TryParse(target, out _))
        {
            _pointer++;
            return;
        }

        Registers[target] += addValue;
        _pointer++;
    }

    private void ProcessCopy(string copyValueString, string target)
    {
        if (long.TryParse(target, out _))
        {
            _pointer++;
            return;
        }

        long copyValue = ParseValue(copyValueString);
        Registers[target] = copyValue;
        _pointer++;
    }

    private void ProcessOut(string outValueString)
    {
        long outValue = ParseValue(outValueString);
        ClockSignal?.Invoke(this, outValue);
        _pointer++;
    }
}

