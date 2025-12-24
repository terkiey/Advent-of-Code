using System;
using System.Security.Cryptography;
using System.Text;

namespace AoC.Days;

internal class OneTimePadCalculator
{
    private string Salt { get; set; } = string.Empty;
    private bool PartOneComplete { get; set; } = false;
    private bool PartTwoComplete { get; set; } = false;

    private Dictionary<int, char> ThreePeats = [];
    private Dictionary<int, char> FivePeats = [];

    public event EventHandler<int>? Key64Made;

    public void ConfigureSalt(string salt)
    {
        Salt = salt;
    }

    public void MakeKeysForPartOne()
    {
        int index = 0;
        while (PartOneComplete == false)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(Salt + index);
            byte[] hashBytes = MD5.HashData(inputBytes);
            string hash = Convert.ToHexString(hashBytes).ToLower();
            ProcessIfRepeating(hash, index);
            index++;
        }
    }

    public void ClearResults()
    {
        ThreePeats.Clear();
        FivePeats.Clear();
    }

    public void MakeStretchedKeysForPartTwo()
    {
        int index = 0;
        while (PartTwoComplete == false)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(Salt + index);
            byte[] hashBytes = MD5.HashData(inputBytes);
            string hash = Convert.ToHexString(hashBytes).ToLower();
            inputBytes = Encoding.UTF8.GetBytes(hash);
            for (int hashIndex = 0; hashIndex < 2016; hashIndex++)
            {
                hashBytes = MD5.HashData(inputBytes);
                hash = Convert.ToHexString(hashBytes).ToLower();
                inputBytes = Encoding.UTF8.GetBytes(hash);
            }
            ProcessIfRepeating(hash, index);
            index++;
        }
    }

    private void ProcessIfRepeating(string hash, int index)
    {
        if (hash.Length < 3)
        {
            return;
        }

        bool threePeatFound = false;
        for (int charIndex = 0; charIndex < hash.Length - 2; charIndex++)
        {
            char hashChar = hash[charIndex];
            if (threePeatFound == false && hash[charIndex .. (charIndex + 3)] == new string(hashChar, 3))
            {
                AddThreePeat(hashChar, index);
                threePeatFound = true;
            }

            if (charIndex < hash.Length - 4 && hash[charIndex  .. (charIndex + 5)] == new string(hashChar, 5))
            {
                AddFivePeat(hashChar, index);
                break;
            }
        }
    }

    private void AddThreePeat(char hashChar, int index)
    {
        if (!ThreePeats.TryAdd(index, hashChar))
        {
            ThreePeats[index] = hashChar;
        }
        CheckPartDone();
    }

    private void AddFivePeat(char hashChar, int index)
    {
        if (!FivePeats.TryAdd(index, hashChar))
        {
            FivePeats[index] = hashChar;
        }
        CheckPartDone();
    }

    private void CheckPartDone()
    {
        if (ThreePeats.Count < 64)
        {
            return;
        }

        if (FivePeats.Count < 1)
        {
            return;
        }

        int oneTimeKeys = 0;
        foreach (var threeKvp in ThreePeats)
        {
            char currentChar = threeKvp.Value;
            if (!FivePeats.ContainsValue(currentChar))
            {
                continue;
            }
            
            int maxFivePeat = threeKvp.Key + 1000;
            foreach (var fiveKvp in FivePeats)
            {
                if (fiveKvp.Value == currentChar && fiveKvp.Key > threeKvp.Key && fiveKvp.Key <= maxFivePeat)
                {
                    oneTimeKeys++;
                    if (oneTimeKeys == 64)
                    {
                        ProcessPartCompletion(threeKvp.Key);
                    }
                    break;
                }
            }
        }
    }

    private void ProcessPartCompletion(int keyIndex)
    {
        if (PartOneComplete == false)
        {
            PartOneComplete = true;
            Key64Made?.Invoke(this, keyIndex);
        }
        else if (PartTwoComplete == false)
        {
            PartTwoComplete = true;
            Key64Made?.Invoke(this, keyIndex);
        }
    }
}
