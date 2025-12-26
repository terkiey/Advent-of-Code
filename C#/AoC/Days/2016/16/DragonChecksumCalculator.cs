using System.Text;

namespace AoC.Days;

internal class DragonChecksumCalculator
{
    public string DragonExtendAndChecksum(string data, int diskSize)
    {
        while(data.Length < diskSize)
        {
            data = DragonExtendStep(data);
        }

        data = data[..diskSize];

        while (data.Length % 2 == 0)
        {
            data = Checksum(data);
        }
        return data;
    }

    private string DragonExtendStep(string data)
    {
        char[] dataChars = data.ToCharArray();
        char[] otherHalf = new char[dataChars.Length];
        for (int charIndex = dataChars.Length - 1; charIndex >= 0; charIndex--)
        {
            otherHalf[dataChars.Length - 1 - charIndex] = dataChars[charIndex] == '1' ? '0' : '1';
        }
        StringBuilder sb = new StringBuilder();
        sb.Append(dataChars);
        sb.Append('0');
        sb.Append(otherHalf);
        return sb.ToString();
    }

    private string Checksum(string data)
    {
        if (data.Length % 2 != 0)
        {
            return data;
        }
        char[] checksum = new char[data.Length / 2];
        int pairIndex = 0;
        for (int charIndex = 0; charIndex < data.Length - 1; charIndex += 2)
        {
                checksum[pairIndex++] = data[charIndex] == data[charIndex + 1] ? '1' : '0';
        }
        return new(checksum);
    }
}
