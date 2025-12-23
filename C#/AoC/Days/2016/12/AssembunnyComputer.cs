using System.Reflection;

namespace AoC.Days;

internal class AssembunnyComputer
{
    public long a { get; set; } = 0;
    public long b { get; set; } = 0;
    public long c { get; set; } = 0;
    public long d { get; set; } = 0;

    public void ProcessInstructions(string[] instructions)
    {
        int pointer = 0;
        while (pointer < instructions.Length && pointer >= 0)
        {
            string instruction = instructions[pointer];
            string[] splitInstruction = instruction.Split(' ');
            switch (splitInstruction[0])
            {
                case "cpy":
                    string copyValue = splitInstruction[1];
                    string target = splitInstruction[2];
                    ProcessCopy(copyValue, target);
                    pointer++;
                    break;

                case "inc":
                    target = splitInstruction[1];
                    RegisterAdd(target, 1);
                    pointer++;
                    break;

                case "dec":
                    target = splitInstruction[1];
                    RegisterAdd(target, -1);
                    pointer++;
                    break;

                case "jnz":
                    string check = splitInstruction[1];
                    int jumpSize = int.Parse(splitInstruction[2]);
                    pointer = ProcessJnz(check, jumpSize, pointer);
                    break;
            }
        }
    }

    public void ClearRegisters()
    {
        a = 0;
        b = 0;
        c = 0;
        d = 0;
    }

    private int ProcessJnz(string check, int jumpSize, int pointer)
    {
        if (long.TryParse(check, out long checkLong) && checkLong != 0)
        {
            return pointer += jumpSize;
        }
        else
        {
            switch (check)
            {
                case "a":
                    if (a != 0)
                    {
                        return pointer += jumpSize;
                    }
                    else
                    {
                        return ++pointer;
                    }

                case "b":
                    if (b != 0)
                    {
                        return pointer += jumpSize;
                    }
                    else
                    {
                        return ++pointer;
                    }

                case "c":
                    if (c != 0)
                    {
                        return pointer += jumpSize;
                    }
                    else
                    {
                        return ++pointer;
                    }

                case "d":
                    if (d != 0)
                    {
                        return pointer += jumpSize;
                    }
                    else
                    {
                        return ++pointer;
                    }
            }
        }
        return -1;
    }

    private void RegisterAdd(string target, long addValue)
    {
        switch (target)
        {
            case "a":
                a += addValue;
                break;

            case "b":
                b += addValue;
                break;

            case "c":
                c += addValue;
                break;

            case "d":
                d += addValue;
                break;
        }
    }

    private void ProcessCopy(string copyValue, string target)
    {
        if (long.TryParse(copyValue, out long copyLong))
        {
            switch (target)
            {
                case "a":
                    a = copyLong;
                    break;

                case "b":
                    b = copyLong;
                    break;

                case "c":
                    c = copyLong;
                    break;

                case "d":
                    d = copyLong;
                    break;
            }
        }
        else
        {
            switch (copyValue)
            {
                case "a":
                    copyLong = a;
                    break;

                case "b":
                    copyLong = b;
                    break;

                case "c":
                    copyLong = c;
                    break;

                case "d":
                    copyLong = d;
                    break;
            }

            switch (target)
            {
                case "a":
                    a = copyLong;
                    break;

                case "b":
                    b = copyLong;
                    break;

                case "c":
                    c = copyLong;
                    break;

                case "d":
                    d = copyLong;
                    break;
            }
        }
    }
}
