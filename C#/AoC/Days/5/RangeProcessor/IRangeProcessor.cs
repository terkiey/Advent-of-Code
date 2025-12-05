namespace AoC.Days;

internal interface IRangeProcessor
{
    List<FreshRange> ParseRanges(List<string> rangeStrings);
    List<FreshRange> CombineRanges(List<FreshRange> rangeList);
    List<ulong> KeepFreshIngredients(List<ulong> ingredients, List<FreshRange> rangeList);
}
