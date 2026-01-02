namespace AoC.Days;

internal class Y2017Day13 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        FirewallWalker walker = new();
        walker.InitialisePath(inputLines);
        int walkDelay = 0;
        walker.Walk(walkDelay++, false);
        AnswerOne = walker.Severity.ToString();
        walker.ResetState();

        // Assuming 0 delay doesnt work due to part one asking for its severity...
        /*This brute force sucks, just do the maths instead
        while (!walker.Walk(walkDelay++, true))
        {
            walker.ResetState();
        }*/

        while (walker.WillGetCaught(walkDelay++)) { }
        //while (walker.WillGetCaughtV2(walkDelay++)) { }
        AnswerTwo = (walkDelay - 1).ToString();
    }
}
