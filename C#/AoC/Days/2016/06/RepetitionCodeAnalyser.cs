namespace AoC.Days;

internal class RepetitionCodeAnalyser
{
    public string GetErrorCorrectedCode(string[] repetitions)
    {
        Dictionary<char, int[]> charCounts = [];
        foreach (string repetition in repetitions)
        {
            for (int charIndex = 0; charIndex < repetition.Length; charIndex++)
            {
                char currentChar = repetition[charIndex];
                charCounts.TryAdd(currentChar, new int[repetition.Length]);
                charCounts[currentChar][charIndex]++;
            }
        }

        char[] codeChars = new char[repetitions[0].Length];
        int[] codeCharOccurences = new int[repetitions[0].Length];
        foreach (char character in charCounts.Keys)
        {
            int[] hitCount = charCounts[character];
            for (int codeCharIndex = 0; codeCharIndex < hitCount.Length; codeCharIndex++)
            {
                if(hitCount[codeCharIndex] > codeCharOccurences[codeCharIndex])
                {
                    codeChars[codeCharIndex] = character;
                    codeCharOccurences[codeCharIndex] = hitCount[codeCharIndex];
                }
            }
        }
        return new(codeChars);
    }

    public string GetModifiedErrorCorrectedCode(string[] repetitions)
    {
        Dictionary<char, int[]> charCounts = [];
        foreach (string repetition in repetitions)
        {
            for (int charIndex = 0; charIndex < repetition.Length; charIndex++)
            {
                char currentChar = repetition[charIndex];
                charCounts.TryAdd(currentChar, new int[repetition.Length]);
                charCounts[currentChar][charIndex]++;
            }
        }

        char[] codeChars = new char[repetitions[0].Length];
        int[] codeCharOccurences = [.. Enumerable.Repeat(int.MaxValue, repetitions[0].Length)];
        foreach (char character in charCounts.Keys)
        {
            int[] hitCount = charCounts[character];
            for (int codeCharIndex = 0; codeCharIndex < hitCount.Length; codeCharIndex++)
            {
                if (hitCount[codeCharIndex] < codeCharOccurences[codeCharIndex])
                {
                    codeChars[codeCharIndex] = character;
                    codeCharOccurences[codeCharIndex] = hitCount[codeCharIndex];
                }
            }
        }
        return new(codeChars);
    }
}
