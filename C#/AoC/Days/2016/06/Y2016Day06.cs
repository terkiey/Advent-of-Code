namespace AoC.Days;

internal class Y2016Day06 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        RepetitionCodeAnalyser analyser = new();
        AnswerOne = analyser.GetErrorCorrectedCode(inputLines);
        AnswerTwo = analyser.GetModifiedErrorCorrectedCode(inputLines);
    }
}
