using System.Net;

namespace AoC.Days;

internal class RangeProcessor : IRangeProcessor
{
    public RangeProcessor() { }

    public List<FreshRange> ParseRanges(List<string> rangeStrings)
    {
        List<FreshRange> rangeList = [];
        foreach (string rangeString in rangeStrings)
        {
            string[] rangeStringArray = rangeString.Split('-');
            rangeList.Add(new(ulong.Parse(rangeStringArray[0]), ulong.Parse(rangeStringArray[1])));
        }

        return rangeList;
    }

    public List<FreshRange> CombineRanges(List<FreshRange> rangeList)
    {
        int firstIndex = 0;
        int secondIndex = 1;
        List<FreshRange> iterList = rangeList.Select(r => r with { }).ToList();

        List<FreshRange> outList = [];
        while (true)
        {
            if (!TryCombineRangeIndices(iterList, firstIndex, secondIndex, out outList))
            {
                if (firstIndex == iterList.Count - 2)
                {
                    break;
                }

                if (secondIndex == iterList.Count - 1)
                {
                    firstIndex += 1;
                    secondIndex = firstIndex + 1;
                    continue;
                }

                secondIndex += 1;
                continue;
            }

            iterList = outList.Select(r => r with { }).ToList();
            if (firstIndex == iterList.Count - 1)
            {
                break;
            }
            secondIndex = firstIndex + 1;
        }

        return outList;
    }

    public List<ulong> KeepFreshIngredients(List<ulong> ingredients, List<FreshRange> rangeList)
    {
        List<ulong> freshList = [];
        foreach (ulong ingredient in ingredients)
        {
            foreach (FreshRange range in rangeList)
            {
                bool ingredientInRange = range.Contains(ingredient);
                if (ingredientInRange)
                {
                    freshList.Add(ingredient);
                    break;
                }
            }
        }

        return freshList;
    }

    private bool TryCombineRangeIndices(List<FreshRange> rangeList, int firstRangeIndex, int secondRangeIndex, out List<FreshRange> newList)
    {
        newList = rangeList.Select(r => r with { }).ToList();
        FreshRange firstRange = rangeList[firstRangeIndex];
        FreshRange secondRange = rangeList[secondRangeIndex];

        if (!firstRange.TryCombine(secondRange, out FreshRange newRange))
        {
            return false;
        }

        newList.Remove(secondRange);
        newList.Remove(firstRange);
        newList.Insert(firstRangeIndex, newRange);
        return true;
    }
}
