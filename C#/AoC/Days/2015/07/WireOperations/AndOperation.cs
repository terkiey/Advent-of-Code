namespace AoC.Days;

internal record AndOperation(string OutWire, string InWire1, string InWire2) : WireOperation(OutWire)
{
}
