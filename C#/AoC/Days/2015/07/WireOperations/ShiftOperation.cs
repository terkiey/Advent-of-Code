namespace AoC.Days;

internal record ShiftOperation(string OutWire, string InWire, bool IsRightShift, ushort ShiftValue) : WireOperation(OutWire)
{
}
