namespace AoC.Days;

internal class Y2015Day05 : Day
{
    static readonly char[] _vowels = ['a', 'e', 'i', 'o', 'u'];
    static readonly string[] _badStrings = ["ab", "cd", "pq", "xy"];
    protected override void RunLogic(string[] inputLines)
    {
        int niceStrings = 0;
        int nicerStrings = 0;
        foreach (string line in inputLines)
        {
            if (IsNice(line)) {  niceStrings++; }
            if (IsNicer(line)) { nicerStrings++; }
        }
        AnswerOne = niceStrings.ToString();
        AnswerTwo = nicerStrings.ToString();
    }

    private bool IsNice(string line)
    {
        char prevChar = char.MinValue;
        int vowels = 0;
        bool doubleLetter = false;
        bool badString = false;
        for (int charIndex = 0; charIndex < line.Length; charIndex++)
        {
            char currentChar = line[charIndex];
            if (vowels < 3 && _vowels.Contains(currentChar))
            {
                vowels++;
            }

            if (prevChar != char.MinValue && doubleLetter == false && currentChar == prevChar)
            {
                doubleLetter = true;
            }

            if (prevChar != char.MinValue && _badStrings.Contains(prevChar.ToString() + currentChar.ToString()))
            {
                badString = true;
                break;
            }
            prevChar = currentChar;
        }

        if (vowels >= 3 && doubleLetter == true && badString == false)
        {
            return true;
        }
        return false;
    }

    private bool IsNicer(string line)
    {
        if (line.Length < 4) 
        { 
            return false; 
        }
        bool pairTwice = false;
        for (int pairIndex = 0; pairIndex < line.Length - 3; pairIndex++)
        {
            if (pairTwice)
            {
                break;
            }
            string pair = line[pairIndex].ToString() + line[pairIndex + 1].ToString();
            for (int nextPairIndex = pairIndex + 2; nextPairIndex < line.Length - 1; nextPairIndex++)
            {
                string nextPair = line[nextPairIndex].ToString() + line[nextPairIndex + 1].ToString();
                if (pair == nextPair)
                {
                    pairTwice = true;
                    break;
                }
            }
        }
        if (pairTwice == false)
        {
            return false;
        }

        for (int charIndex = 0; charIndex < line.Length - 2; charIndex++)
        {
            if (line[charIndex] == line[charIndex +2])
            {
                return true;
            }
        }

        return false;
    }
}
