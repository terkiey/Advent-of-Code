using System.Text;

namespace AoC.Days;

internal class KnotHasher
{
    public string GetKnotHash(List<int> sparseHash)
    {
        List<int> denseHash = Densify(sparseHash);
        StringBuilder sb = new();
        foreach (int number in denseHash)
        {
            sb.Append(number.ToString("x2"));    
        }
        return sb.ToString();
    }

    private List<int> Densify(List<int> sparseHash)
    {
        if (sparseHash.Count % 16 != 0)
        {
            return [];
        }

        List<int> blocks = [];
        int blockCount = sparseHash.Count / 16;
        for (int blockIndex = 0; blockIndex < blockCount; blockIndex++)
        {
            var block = sparseHash[(blockIndex * 16)..(blockIndex * 16 + 16)];
            int blockVal = block[0];
            for (int numIndex = 1; numIndex < 16;  numIndex++)
            {
                blockVal ^= block[numIndex];
            }
            blocks.Add(blockVal);
        }
        return blocks;
    }
}
