namespace AoC.Days;

internal class Y2017Day23 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        ExperimentalCoprocessor processor = new();
        processor.InitialiseComputerState();
        processor.RunProgram(inputLines);
        AnswerOne = processor.MulInvocationCounter.ToString();

        /* The brute force method, will never run in reasonable time */
        //processor.InitialiseComputerState();
        //processor.Registers['a'] = 1;
        //processor.RunProgram(inputLines);
        //AnswerTwo = processor.Registers['h'].ToString();

        /* To save me writing a peephole optimising compiler, I just did the peephole optimisation in peephole.png in this day's folder,
         * my input (and I assume other inputs) came out to "count the number of nonprime numbers you encounter when counting from b to c inclusive in jumps of x"
         * In my case that range was b = 105,700 and c = 122,700 and jumpsize x = 17.
         */

        int registerHValue = 0;
        int startValue = 105700;
        int endValue = 122700;
        int jumpSize = 17;
        int pointer = startValue;

        while (pointer <= endValue)
        {
            int highestFactorToCheck = (int)Math.Ceiling(Math.Sqrt(pointer));
            for (int checkFactor = 2; checkFactor <= highestFactorToCheck; checkFactor++)
            {
                if (pointer % checkFactor == 0)
                {
                    registerHValue++;
                    break;
                }
            }
            pointer += jumpSize;
        }
        AnswerTwo = registerHValue.ToString();
    }
}
