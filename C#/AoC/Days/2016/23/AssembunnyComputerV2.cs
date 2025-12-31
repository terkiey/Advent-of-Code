using System.Reflection;

namespace AoC.Days;

// I briefly considered extending the original assembunny computer as that seemed to be intended, but then it would require a bit of a refactor, which is unnecessary for throwaway puzzle solutions.
// So I just copy pasted the code here and changed it as I wanted.

internal class AssembunnyComputerV2
{
    private long _pointer = 0;
    private Dictionary<long, int> ToggleCount = [];
    private long _lastJumpPointer = -1;
    private long[] _lastJumpRegisters = new long[4];

    public long a { get; set; } = 0;
    public long b { get; set; } = 0;
    public long c { get; set; } = 0;
    public long d { get; set; } = 0;

    public void ProcessInstructions(string[] instructions)
    {
        _pointer = 0;
        ToggleCount = [];
        while (_pointer < instructions.Length && _pointer >= 0)
        {
            string instruction = instructions[_pointer];
            string[] splitInstruction = instruction.Split(' ');
            switch (splitInstruction[0])
            {
                case "cpy":
                    string copyValue = splitInstruction[1];
                    string target = splitInstruction[2];
                    ProcessCopy(copyValue, target, true);
                    break;

                case "inc":
                    target = splitInstruction[1];
                    RegisterAdd(target, 1, true);
                    break;

                case "dec":
                    target = splitInstruction[1];
                    RegisterAdd(target, -1, true);
                    break;

                case "jnz":
                    string check = splitInstruction[1];
                    string jumpSize = splitInstruction[2];
                    ProcessJnz(check, jumpSize, true);
                    break;

                case "tgl":
                    ProcessTgl(splitInstruction[1], true);
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

    private void ProcessTgl(string offsetString, bool checkToggles)
    {
        if (checkToggles == true && ToggleCount.TryGetValue(_pointer, out int toggles))
        {
            if (toggles % 2 == 0)
            {
                RegisterAdd(offsetString, -1, false);
            }
            else
            {
                RegisterAdd(offsetString, 1, false);
            }
            _pointer++;
            return;
        }

        // I should really turn the parsing into its own method... oh well.
        long offset = 0;
        switch (offsetString)
        {
            case "a":
                offset = a;
                break;

            case "b":
                offset = b;
                break;

            case "c":
                offset = c;
                break;

            case "d":
                offset = d;
                break;
        }

        long target = _pointer + offset;
        if (!ToggleCount.TryAdd(target, 1))
        {
            ToggleCount[target]++;
        }
        _pointer++;
    }

    // This jnz method is the best example that I should just use a dictionary to hold the register vals to reduce control flow statements... oh well.
    private void ProcessJnz(string check, string jumpSizeString, bool checkToggles)
    {
        if (checkToggles == true && ToggleCount.TryGetValue(_pointer, out int toggles) && toggles % 2 != 0)
        {
            ProcessCopy(check, jumpSizeString, false);
            return;
        }

        if (!long.TryParse(check, out long checkLong))
        {
            switch (check)
            {
                case "a":
                    checkLong = a;
                    break;

                case "b":
                    checkLong = b;
                    break;

                case "c":
                    checkLong = c;
                    break;

                case "d":
                    checkLong = d;
                    break;
            }
        }

        if (_pointer == _lastJumpPointer)
        {
            long loopCount = 0;
            long[] changes = [a - _lastJumpRegisters[0], b - _lastJumpRegisters[1], c - _lastJumpRegisters[2], d - _lastJumpRegisters[3]];
            switch (check)
            {
                case "a":
                    loopCount = Math.Abs(a / changes[0]);
                    break;

                case "b":
                    loopCount = Math.Abs(b / changes[1]);
                    break;

                case "c":
                    loopCount = Math.Abs(c / changes[2]);
                    break;

                case "d":
                    loopCount = Math.Abs(d / changes[3]);
                    break;
            }

            a += changes[0] * loopCount;
            b += changes[1] * loopCount;
            c += changes[2] * loopCount;
            d += changes[3] * loopCount;
            _pointer++;
            return;
        }

        if (!long.TryParse(jumpSizeString, out long jumpSize))
        {
            switch (jumpSizeString)
            {
                case "a":
                    jumpSize = a;
                    break;

                case "b":
                    jumpSize = b;
                    break;

                case "c":
                    jumpSize = c;
                    break;

                case "d":
                    jumpSize = d;
                    break;
            }
        }

        if (checkLong == 0)
        {
            jumpSize = 1;
        }
        _lastJumpPointer = _pointer;
        _lastJumpRegisters = [a, b, c, d];
        _pointer += jumpSize;
    }

    private void RegisterAdd(string target, long addValue, bool checkToggles)
    {
        if (checkToggles == true && ToggleCount.TryGetValue(_pointer, out int toggles) && toggles % 2 != 0)
        {
            addValue *= -1;
        }

        if (long.TryParse(target, out _))
        {
            _pointer++;
            return;
        }

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
        _pointer++;
    }

    private void ProcessCopy(string copyValue, string target, bool checkToggles)
    {
        if (checkToggles == true && ToggleCount.TryGetValue(_pointer, out int toggles) && toggles % 2 != 0)
        {
            ProcessJnz(copyValue, target, false);
            return;
        }

        if (long.TryParse(target, out _))
        {
            _pointer++;
            return;
        }

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
        _pointer++;
    }
}


