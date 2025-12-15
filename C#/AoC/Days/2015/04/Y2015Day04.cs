using System.Security.Cryptography;
using System.Text;

namespace AoC.Days;

internal class Y2015Day04 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        string inputKey = inputLines[0];
        int number = 1;
        while (true)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(inputKey + number);

            byte[] md5 = MD5.HashData(keyBytes);

            if (md5[0] == 0 && md5[1] == 0 && md5[2] < 16)
            {
                AnswerOne = AnswerOne == String.Empty ? number.ToString() : AnswerOne;
            }

            if (md5[0] == 0 && md5[1] == 0 && md5[2] == 0)
            {
                AnswerTwo = number.ToString();
                break;
            }

            number++;
        }
    }
}
