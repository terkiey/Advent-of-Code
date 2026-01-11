namespace AoC.Days;

internal class DuetComputer
{
    // Cpus
    private int Pointer = 0;
    private Dictionary<int, DuetProcessor> Cpus { get; } = new()
    {
        { 0, new() },
        { 1, new() },
    };
    private Dictionary<int, Queue<long>> MessageQueues = new()
    {
        { 0, new() },
        { 1, new() },
    };
    private Dictionary<int, bool> CpuTerminations = new()
    {
        { 0, false },
        { 1, false },
    };
    private Dictionary<int, bool> CpuWaiting = new()
    {
        { 0, false },
        { 1, false },
    };

    // AoC
    private long FirstProgramZeroRecover = -1;
    private long ProgramOneSends = 0;

    public string PartOneAnswer => FirstProgramZeroRecover.ToString();
    public string PartTwoAnswer => ProgramOneSends.ToString();

    public void ParseInstructions(string[] instructions)
    {
        Cpus[0].ParseInstructions(instructions);
        Cpus[1].ParseInstructions(instructions);
    }

    public void RunSoloMode()
    {
        ResetAllState();
        Cpus[0].RunUntilSend();
        Cpus[0].RunUntilRecover();
        FirstProgramZeroRecover = MessageQueues[1].Last();
    }

    public void RunDuetMode()
    {
        ResetAllState();
        Dictionary<int, Func<bool>> CpuLocks = new()
        {
            { 0, () => CpuTerminations[0] || CpuWaiting[0] },
            { 1, () => CpuTerminations[1] || CpuWaiting[1] },
        };

        foreach (var kvp in Cpus)
        {
            Cpus[kvp.Key].Registers['p'] = kvp.Key;
        }

        while (!CpuLocks[0]() || !CpuLocks[1]())
        {
            while(CpuLocks[Pointer]() == false)
            {
                if (MessageQueues[Pointer].Count == 0)
                {
                    CpuWaiting[Pointer] = true;
                }
                if (!Cpus[Pointer].RunUntilRecover())
                {
                    CpuTerminations[Pointer] = true;
                }
                if (CpuWaiting[Pointer])
                {
                    Cpus[Pointer].Pointer--;
                }

            }
            if (MessageQueues[Pointer ^= 1].Count > 0)
            {
                CpuWaiting[Pointer] = false;
            }
        }
    }

    public void ConnectEvents()
    {
        foreach (var kvp in Cpus)
        {
            Cpus[kvp.Key].Recover += RecoverHandler;
            Cpus[kvp.Key].Sound += SoundHandler;
        }
    }

    private void SoundHandler(object? sender, long frequency)
    {
        MessageQueues[Pointer ^ 1].Enqueue(frequency);
        if (Pointer == 1)
        {
            ProgramOneSends++;
        }
    }
    
    private void RecoverHandler(object? sender, char register)
    {
        if (MessageQueues[Pointer].Count == 0)
        {
            return;
        }
        Cpus[Pointer].Registers[register] = MessageQueues[Pointer].Dequeue();
    }

    private void ResetAllState()
    {
        Pointer = 0;
        foreach (var kvp in Cpus)
        {
            Cpus[kvp.Key].ResetState();
            MessageQueues[kvp.Key].Clear();
        }
    }
}
