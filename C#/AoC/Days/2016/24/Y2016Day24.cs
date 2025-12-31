namespace AoC.Days;

internal class Y2016Day24 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        AirDuctMazer mazer = new AirDuctMazer();
        mazer.ConstructMaze(inputLines);
        int[] answers = mazer.FewestStepsVisitAllNumbers();
        AnswerOne = answers[0].ToString();
        AnswerTwo = answers[1].ToString();
    }
}
