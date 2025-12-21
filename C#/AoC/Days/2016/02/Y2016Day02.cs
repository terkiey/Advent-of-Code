namespace AoC.Days;

internal class Y2016Day02 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        KeypadTraverser keypad = new();
        AnswerOne = keypad.GetBathroomCode(inputLines);
        AnswerTwo = keypad.GetFuckedBathroomCode(inputLines);
    }
}
