namespace Day1;


internal class Safe : ISafe
{
    public int Dial { get; private set; }

    public event EventHandler<int>? DialClicked;
    public event EventHandler<int>? DialStopped;

    public Safe(int dialStart)
    {
        Dial = dialStart;
    }

    public List<int> ReadInstructions(string[] instructions)
    {
        List<int> dialTurns = [];
        foreach (string instruction in instructions)
        {
            string turnSizeString = instruction.Substring(1);
            int turnSize = int.Parse(turnSizeString);

            if (instruction.StartsWith("R"))
            {
                dialTurns.Add(turnSize);
            }
            else
            {
                dialTurns.Add(turnSize * -1);
            }
        }

        return dialTurns;
    }

    public void TurnDial(int dialTurn)
    {
        for (int click = 0; click < Math.Abs(dialTurn); click++)
        {
            if (dialTurn > 0) { DialClick(true); }
            else { DialClick(false); }
        }
        
        Dial %= 100;
        DialStopped?.Invoke(this, Dial);
        
    }

    private void DialClick(bool upwards)
    {
        if (upwards)
        {
            Dial += 1;
            DialClicked?.Invoke(this, Dial);
        }
        else
        {
            Dial -= 1;
            DialClicked?.Invoke(this, Dial);
        }
    }
}
