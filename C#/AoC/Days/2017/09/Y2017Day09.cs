namespace AoC.Days;

internal class Y2017Day09 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        CharacterStreamGroupManager manager = new();
        manager.ParseStream(inputLines[0]);
        AnswerOne = manager.TotalScore().ToString();
        AnswerTwo = manager.GarbageCharacters.ToString();
    }
}
