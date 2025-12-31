namespace AoC.Days;

internal class Y2016Day25 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        // After reading the input sheet closely for a while,and writing what the jump blocks are actually doing on paper I realise that this pattern does the following;
        /* Take 2532 + startRegister, divide it by 2 and signal 0 or 1 for the remainder. (the starting seed depends on the early lines that copy to registers b and c but I cba parsing it).
         * Repeat this until the division gives 0 (which you output)
         * Then start at 2532 and repeat the process forever.
         */

        // That means, the question is really, which is the lowest number whose bits are alternating 0,1,0,1 ending on 1 that is above 2532.
        bool validNumberFound = false;
        int startRegister = 1;
        while (!validNumberFound)
        {
            int seed = 2532 + startRegister;
            int lastRemainder = 1;
            while (!validNumberFound)
            {
                int remainder = seed & 1;
                if (remainder == lastRemainder)
                {
                    startRegister++;
                    break;
                }

                if ((seed >>= 1) == 0)
                {
                    if (remainder == 1)
                    {
                        validNumberFound = true;
                        break;
                    }
                    startRegister++;
                    break;
                }
                lastRemainder = remainder;
            }
        }
        AnswerOne = startRegister.ToString();
    }

}
