namespace AoC.Days;

/// <summary>
/// Translates instructions and keeps track of the dial position after making turns. Also broadcasts dial turn events to be solved elsewhere.
/// </summary>
public interface ISafe
{
    int Dial { get; }

    event EventHandler<int>? DialStopped;
    event EventHandler<int>? DialClicked;

    List<int> ReadInstructions(string[] instructions);
    void TurnDial(int dialTurn);

}
