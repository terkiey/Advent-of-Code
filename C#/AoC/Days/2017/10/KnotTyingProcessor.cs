using System;
using System.Text;

namespace AoC.Days;

internal class KnotTyingProcessor
{
    public List<int> Numbers { get; } = [];

    private int Pointer { get; set; } = 0;
    private int Offset { get; set; } = 0;

    private List<int> ByteSeed { get; } = [17, 31, 73, 47, 23];

    public void ProcessInstructions(string instructionsString, int numberCount, bool takeAsBytes, int rounds)
    {
        Pointer = 0;
        Offset = 0;
        Numbers.Clear();

        for (int number = 0; number < numberCount; number++)
        {
            Numbers.Add(number);
        }

        IEnumerable<int> instructions = [];
        if (takeAsBytes)
        {
            instructions = ParseAsBytes(instructionsString);
        }
        else
        {
            instructions = ParseAsLengths(instructionsString);
        }

        for (int roundIndex = 0; roundIndex < rounds; roundIndex++)
        {
            foreach (int instruction in instructions)
            {
                ProcessInstruction(instruction);
            }
        }
    }

    private IEnumerable<int> ParseAsBytes(string instructions)
    {
        foreach (char c in instructions)
        {
            yield return (int)c;
        }
        foreach (int num in ByteSeed)
        {
            yield return num;
        }
    }

    private IEnumerable<int> ParseAsLengths(string instructions)
    {
        string[] splitInstructions = instructions.Split(',');
        foreach (string instruction in splitInstructions)
        {
            yield return int.Parse(instruction);
        }
    }

    private void ProcessInstruction(int length)
    {
        List<int> Span = [];
        if (length + Pointer < Numbers.Count)
        {
            Span = Numbers[Pointer..(Pointer + length)];
        }
        else
        {
            Span = Numbers[Pointer..];
            Span.AddRange(Numbers[..(length - (Numbers.Count - Pointer))]);
        }

        for (int spanIndex = Span.Count - 1; spanIndex >= 0; spanIndex--)
        {
            int index = Span.Count - 1 - spanIndex;
            Numbers[(Pointer + index) % Numbers.Count] = Span[spanIndex];
        }
        Pointer += length + Offset++;
        Pointer %= Numbers.Count;
    }
}
