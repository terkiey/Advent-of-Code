namespace AoC.Days;

internal record OrOperation(string OutWire, string InWire1, string InWire2) : WireOperation(OutWire)
{
}
