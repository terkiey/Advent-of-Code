using Google.OrTools.ConstraintSolver;

namespace AoC.Days;

internal class Y2018Day02 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        int threePeat = 0;
        int twoPeat = 0;
        foreach (string ID in inputLines)
        {
            int[] charCounts = new int[26];
            foreach (char character in ID)
            {
                charCounts[character - 'a']++;
            }

            bool twoPeatCounted = false;
            bool threePeatCounted = false;
            foreach(var count in charCounts)
            {
                if (twoPeatCounted && threePeatCounted)
                {
                    break;
                }

                if (!twoPeatCounted)
                {
                    if (count == 2) 
                    { 
                        twoPeat++;
                        twoPeatCounted = true;
                        continue; 
                    }
                }
                if (!threePeatCounted)
                {
                    if (count == 3) 
                    { 
                        threePeat++;
                        threePeatCounted = true;
                        continue; 
                    }
                }
            }
        }
        AnswerOne = (threePeat * twoPeat).ToString();

        for(int indexOne = 0; indexOne < inputLines.Length - 1; indexOne++)
        {
            string boxOne = inputLines[indexOne];
            for (int indexTwo = indexOne + 1; indexTwo < inputLines.Length; indexTwo++)
            {
                int failIndex = -1;
                string boxTwo = inputLines[indexTwo];
                for (int charIndex = 0; charIndex < boxTwo.Length; charIndex++)
                {
                    if (boxOne[charIndex] == boxTwo[charIndex])
                    {
                        continue;
                    }

                    if (failIndex == -1)
                    {
                        failIndex = charIndex;
                    }
                    else
                    {
                        failIndex = -1;
                        break;
                    }
                }
                if (failIndex != -1)
                {
                    AnswerTwo = boxOne[..failIndex] + boxOne[(failIndex + 1)..];
                }
            }
        }
    }
}
