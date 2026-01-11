namespace AoC.Days;

public enum MoveType
{
    Spin,
    Exchange,
    Partner
}

public struct DanceMoveStruct
{
    public MoveType Type;
    public int Arg1;
    public int Arg2;
    public char Char1;
    public char Char2;

    public DanceMoveStruct(int arg1)
    {
        Type = MoveType.Spin;
        Arg1 = arg1;
    }

    public DanceMoveStruct(int arg1, int arg2)
    {
        Type = MoveType.Exchange;
        Arg1 = arg1;
        Arg2 = arg2;
    }

    public DanceMoveStruct(char char1, char char2)
    {
        Type = MoveType.Partner;
        Char1 = char1;
        Char2 = char2;
    }
}