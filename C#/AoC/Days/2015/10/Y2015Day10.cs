using System.Text;
using System.Text.RegularExpressions;

namespace AoC.Days;

internal class Y2015Day10 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        string iterationValue = inputLines[0];
        for (int i = 0; i < 40; i++)
        {
            iterationValue = LookAndSay(iterationValue);
        }
        AnswerOne = iterationValue.Length.ToString();

        for (int i = 0; i < 10; i++)
        {
            iterationValue = LookAndSay(iterationValue);
        }
        AnswerTwo = iterationValue.Length.ToString();
    }

    private string LookAndSay(string numString)
    {
        var sayStringBuilder = new StringBuilder(numString.Length * 2);
        int charCount = 1;
        char prevChar = numString[0];
        for (int charIndex = 1;  charIndex < numString.Length; charIndex++)
        {
            char currentChar = numString[charIndex];
            if (currentChar == prevChar)
            {
                charCount++;
            }
            else
            {
                sayStringBuilder.Append(charCount);
                sayStringBuilder.Append(prevChar);
                charCount = 1;
                prevChar = currentChar;
            }
        }
        sayStringBuilder.Append(charCount);
        sayStringBuilder.Append(prevChar);
        return sayStringBuilder.ToString();
    }
}
