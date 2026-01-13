namespace AoC.Days;

internal class Y2017Day21 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        int enhanceCount = RunParameter == 1 ? 5 : 2;
        FractalTilerMachine machine = new(inputLines);
        machine.Enhance(enhanceCount);
        AnswerOne = machine.GetOnPixelCount().ToString();
        machine.Enhance(13);
        AnswerTwo = machine.GetOnPixelCount().ToString();
    }
}
/* TODO_MID: Now that I know even in part two all that matter is the count of pixels on...
 * I could simply keep track of how many of each tile I have, and iterate them in a Dictionary...
 * 
 * That would reduce the time spent on this exponentially because if I know I have 8 of a tile, then thats 8 of the output, without only one calculation.
 */