using System.Text;

namespace AoC.Days;

internal class Y2015Day08 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        int codeVsMemoryChars = 0;
        foreach (string line in inputLines)
        {
            int codeChars = line.Length;
            string processedString = line.Substring(1, codeChars - 2);
            for (int charIndex = 0; charIndex < processedString.Length; charIndex++)
            {
                char currentChar = processedString[charIndex];
                if (currentChar == '\\')
                {
                    char nextChar = processedString[charIndex + 1];
                    switch (nextChar)
                    {
                        case 'x':
                            string hexString = processedString.Substring(charIndex + 2, 2);
                            byte[] hexBytes = Convert.FromHexString(hexString);
                            string hexChar = Encoding.ASCII.GetString(hexBytes);
                            processedString = processedString.Substring(0, charIndex) + hexChar + processedString.Substring(charIndex + 4);
                            break;

                        case '\\':
                            processedString = processedString.Substring(0, charIndex) + "\\" + processedString.Substring(charIndex + 2);
                            break;

                        case '"':
                            processedString = processedString.Substring(0, charIndex) + "\"" + processedString.Substring(charIndex + 2);
                            break;
                    }
                }
            }
            int memoryChars = processedString.Length;
            codeVsMemoryChars += codeChars;
            codeVsMemoryChars -= memoryChars;
        }
        AnswerOne = codeVsMemoryChars.ToString();

        int codeVsEncodedChars = 0;
        foreach (string line in inputLines)
        {
            int codeChars = line.Length;
            int encodedChars = line.Length + 2;
            foreach(char currentChar in line)
            {
                if (currentChar == '\\' || currentChar == '"')
                {
                    encodedChars++;
                }
            }
            codeVsEncodedChars -= codeChars;
            codeVsEncodedChars += encodedChars;
        }
        AnswerTwo = codeVsEncodedChars.ToString();
    }
}
