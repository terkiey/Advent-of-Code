namespace AoC.Days;

internal class Y2017Day10 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        KnotTyingProcessor knotter = new();
        KnotHasher hasher = new();
        int numCount = -1;
        switch (RunParameter)
        {
            case 1:
                numCount = 256;
                break;

            case 2:
                numCount = 5;
                break;
        }
        knotter.ProcessInstructions(inputLines[0], numCount, false, 1);
        AnswerOne = (knotter.Numbers[0] * knotter.Numbers[1]).ToString();
        knotter.ProcessInstructions(inputLines[0], numCount, true, 64);
        AnswerTwo = hasher.GetKnotHash(knotter.Numbers);
    }
}
