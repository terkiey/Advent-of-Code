namespace Day2;

internal class IdParser : IIdParser
{
    public IdParser() { }
    
    public List<string[]> ParseIdRanges(string inputLine)
    {
        List<string[]> outList = [];
        string[] array = new string[2];

        string[] rangeStrings = inputLine.Split(",");
        foreach (string rangeString in rangeStrings)
        {
            array = rangeString.Split("-");
            outList.Add(array);
        }

        return outList;
    }
}
