namespace AoC.Days;

internal class Y2025Day5 : Day
{
    protected override void RunLogic(string[] lines)
    {  
        List<ulong> ingredients = [];
        List<string> rangeLines = [];
        bool stillOnRangeLines = true;
        foreach(string line in lines)
        {
            if (stillOnRangeLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    stillOnRangeLines= false;
                    continue;
                }

                rangeLines.Add(line);
                continue;
            }

            ingredients.Add(ulong.Parse(line));
        }

        IRangeProcessor _rangeProcessor = new RangeProcessor();
        List<FreshRange> rangeList = _rangeProcessor.ParseRanges(rangeLines);
        List<FreshRange> combinedRangeList = _rangeProcessor.CombineRanges(rangeList);
        List<ulong> freshList = _rangeProcessor.KeepFreshIngredients(ingredients, combinedRangeList);

        AnswerOne = freshList.Count().ToString();

        ulong AnswerTwoNum = 0;
        foreach (FreshRange range in combinedRangeList)
        {
            AnswerTwoNum += range.Size;
        }

        AnswerTwo = AnswerTwoNum.ToString();
    }
}
