using System.Text;

namespace AoC.Days;

internal class AlchemicalReductionChemicalFacility
{
    public string ReducePolymer(string polymer)
    {
        // Write out the polymer, and whenever the last two characters are reactable, just delete them both and keep writing.
        List<char> reducedPolymerChars = [];
        for (int charIndex = 0; charIndex < polymer.Length; charIndex++)
        {
            reducedPolymerChars.Add(polymer[charIndex]);
            if (reducedPolymerChars.Count < 2)
            {
                continue;
            }

            if ((char.ToLower(reducedPolymerChars[^1]) == char.ToLower(reducedPolymerChars[^2])) && (char.IsLower(reducedPolymerChars[^1]) != char.IsLower(reducedPolymerChars[^2])))
            {
                reducedPolymerChars.RemoveAt(reducedPolymerChars.Count - 1);
                reducedPolymerChars.RemoveAt(reducedPolymerChars.Count - 1);
            }
        }
        return new([..reducedPolymerChars]);
    }

    // This is a reduction except we are allowed to pick a letter and remove all instances of it, then reduce.
    public int PowerReducePolymer(string polymer)
    {
        // Firstly, we know based on the reduction operation that we could just reduce, then remove, then reduce again and thats the same as removing first. (order unchanged).
        // But its more efficient I reckon, less characters to remove each iteration, and potentially less different chars to try.
        string reagent = ReducePolymer(polymer);
        HashSet<char> containedChars = [.. reagent.ToLower()];
        List<char> reagentChar = [.. reagent];
        int minSize = int.MaxValue;
        foreach (char c in containedChars)
        {
            List<char> reagentCharCopy = [.. reagentChar];
            reagentCharCopy.RemoveAll(element => char.ToLower(element) == c);
            string newPolymer = new([.. reagentCharCopy]);
            int reducedSize = ReducePolymer(newPolymer).Length;
            if (reducedSize < minSize)
            {
                minSize = reducedSize;
            }
        }
        return minSize;
    }

}
