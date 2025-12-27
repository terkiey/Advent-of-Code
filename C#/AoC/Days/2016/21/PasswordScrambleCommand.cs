namespace AoC.Days;
internal class PasswordScrambleCommand
{
    public string Verb { get; }
    public string X { get; }
    public string Y { get; }
    public string RotateType { get; set; }

    public PasswordScrambleCommand(string verb, string x, string y, string rotateType)
    {
        Verb = verb;
        X = x;
        Y = y;
        RotateType = rotateType;
    }
}