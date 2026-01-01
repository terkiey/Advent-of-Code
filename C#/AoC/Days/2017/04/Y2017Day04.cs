namespace AoC.Days;

internal class Y2017Day04 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        int failedPhrases = 0;
        List<string> checkAnagrams = [];
        foreach (string passPhrase in inputLines)
        {
            HashSet<string> usedWords = [];
            string[] words = passPhrase.Split(' ');
            bool failed = false;
            foreach (string word in words)
            {
                if (!usedWords.Add(word))
                {
                    failedPhrases++;
                    failed = true;
                    break;
                }
            }
            if (!failed)
            {
                checkAnagrams.Add(passPhrase);
            }
        }
        AnswerOne = (inputLines.Length - failedPhrases).ToString();

        for (int phraseIndex = 0; phraseIndex < checkAnagrams.Count; phraseIndex++)
        {
            string phrase = checkAnagrams[phraseIndex];
            string[] words = phrase.Split(' ');
            bool failed = false;
            for (int wordIndex = 0; wordIndex < words.Length - 1; wordIndex++)
            {
                if (failed)
                {
                    break;
                }
                string word = words[wordIndex];
                for (int otherWordIndex = wordIndex + 1; otherWordIndex < words.Length; otherWordIndex++)
                {
                    string otherWord = words[otherWordIndex];
                    if (word.Length != otherWord.Length)
                    {
                        continue;
                    }

                    if (word.ToCharArray().Order().SequenceEqual(otherWord.ToCharArray().Order()))
                    {
                        failed = true;
                        failedPhrases++;
                        break;
                    }
                }
            }
        }
        AnswerTwo = (inputLines.Length - failedPhrases).ToString();
    }
}
