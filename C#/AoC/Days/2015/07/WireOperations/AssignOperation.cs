namespace AoC.Days;

internal record AssignOperation(string OutWire, string Value) : WireOperation(OutWire)
{
}
