using System;

namespace AoC.Days;

internal class Y2015Day07 : Day
{
    private readonly List<string> _wires = [];
    private readonly List<WireOperation> _operations = [];
    private readonly Dictionary<string, ushort> _wireSignals = [];

    protected override void RunLogic(string[] inputLines)
    {
        foreach (string operation in inputLines)
        {
            WireOperation parsedOperation = ParseOperation(operation);
            _operations.Add(parsedOperation);
            _wires.Add(parsedOperation.OutWire);
        }

        // First attempt, just run every operation in turn, ignoring gates whose inputs are not setup yet, and repeat until we fail to process any operations.
        bool nothingProcessed = false;
        List<WireOperation> operationsCopy = _operations.ToList();
        while (nothingProcessed == false)
        {
            nothingProcessed = true;
            List<WireOperation> iterOperations = operationsCopy.ToList();
            foreach (WireOperation operation in iterOperations)
            {
                if (TryProcessOperation(operation))
                {
                    nothingProcessed = false;
                    operationsCopy.Remove(operation);
                }
            }
        }
        AnswerOne = _wireSignals["a"].ToString();
        _wireSignals.Clear();

        operationsCopy = _operations.ToList();
        nothingProcessed = false;
        while (nothingProcessed == false)
        {
            nothingProcessed = true;
            List<WireOperation> iterOperations = operationsCopy.ToList();
            foreach (WireOperation operation in iterOperations)
            {
                if (TryProcessOperationPart2(operation))
                {
                    nothingProcessed = false;
                    operationsCopy.Remove(operation);
                }
            }
        }
        AnswerTwo = _wireSignals["a"].ToString();
    }

    private WireOperation ParseOperation(string operation)
    {
        string[] splitString = operation.Split(' ');
        switch (splitString[1])
        {
            case "->":
                return new AssignOperation(splitString[2],splitString[0]);

            case "AND":
                return new AndOperation(splitString[4], splitString[0], splitString[2]);

            case "OR":
                return new OrOperation(splitString[4], splitString[0], splitString[2]);

            case "RSHIFT":
                return new ShiftOperation(splitString[4], splitString[0], true, ushort.Parse(splitString[2]));

            case "LSHIFT":
                return new ShiftOperation(splitString[4], splitString[0], false, ushort.Parse(splitString[2]));

            default:
                return new NotOperation(splitString[3], splitString[1]);
        }
    }

    private bool TryProcessOperation(WireOperation operation)
    {
        ushort val1;
        ushort val2;
        switch (operation)
        {
            case AssignOperation assignOp:
                try
                {
                    val1 = ushort.Parse(assignOp.Value);
                    _wireSignals.Add(assignOp.OutWire, val1);
                    return true;
                }
                catch
                {
                    if(!_wireSignals.TryGetValue(assignOp.Value, out val1))
                    {
                        return false;
                    }
                    _wireSignals.Add(assignOp.OutWire, val1);
                    return true;
                }

            case AndOperation andOp:
                try
                {
                    val1 = ushort.Parse(andOp.InWire1);
                }
                catch
                {
                    if (!_wireSignals.TryGetValue(andOp.InWire1, out val1))
                    {
                        return false;
                    }
                }
                try
                {
                    val2 = ushort.Parse(andOp.InWire2);
                }
                catch
                {
                    if (!_wireSignals.TryGetValue(andOp.InWire2, out val2))
                    {
                        return false;
                    }
                }
                _wireSignals.Add(andOp.OutWire, (ushort)(val1 & val2));
                return true;

            case OrOperation orOp:
                try
                {
                    val1 = ushort.Parse(orOp.InWire1);
                }
                catch
                {
                    if (!_wireSignals.TryGetValue(orOp.InWire1, out val1))
                    {
                        return false;
                    }
                }
                try
                {
                    val2 = ushort.Parse(orOp.InWire2);
                }
                catch
                {
                    if (!_wireSignals.TryGetValue(orOp.InWire2, out val2))
                    {
                        return false;
                    }
                }
                _wireSignals.Add(orOp.OutWire, (ushort)(val1 | val2));
                return true;

            case ShiftOperation shiftOp:
                if (!_wireSignals.TryGetValue(shiftOp.InWire, out val1))
                {
                    return false;
                }
                if (shiftOp.IsRightShift)
                {
                    _wireSignals.Add(shiftOp.OutWire, (ushort)(val1 >> shiftOp.ShiftValue));
                    return true;
                }
                else
                {
                    _wireSignals.Add(shiftOp.OutWire, (ushort)(val1 << shiftOp.ShiftValue));
                    return true;
                }

            case NotOperation notOp:
                try
                {
                    val1 = ushort.Parse(notOp.InWire);
                }
                catch
                {
                    if (!_wireSignals.TryGetValue(notOp.InWire, out val1))
                    {
                        return false;
                    }
                }
                _wireSignals.Add(notOp.OutWire, (ushort)~val1);
                return true;
        }
        return false;
    }

    private bool TryProcessOperationPart2(WireOperation operation)
    {
        if (operation.OutWire == "b")
        {
            _wireSignals.Add("b", ushort.Parse(AnswerOne));
            return true;
        }
        return TryProcessOperation(operation);
    }
}
