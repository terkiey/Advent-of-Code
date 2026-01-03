namespace AoC.Days;

internal class Y2017Day15 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        DuelingGeneratorsJudge judge = new();
        judge.GenSeeds(inputLines);
        judge.Duel(40000000);
        AnswerOne = judge.Count.ToString();
        judge.GenSeeds(inputLines);
        judge.DuelTwo(5000000);
        AnswerTwo = judge.Count.ToString();
    }
}
