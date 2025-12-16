namespace AoC.Days;

internal class Y2025Day08: Day
{
    protected override void RunLogic(string[] lines)
    {
        IPlayground playground = new Playground(lines);
        playground.CalculateAllDistances();

        for (int iteration = 1; iteration <= RunParameter; iteration++)
        {
            playground.ConnectClosestPair();
        }

        int AnswerOneNum = 1;
        playground.Circuits.Sort((a, b) => b.CountBoxes.CompareTo(a.CountBoxes));
        for (int index = 0; index < 3; index++)
        {
            AnswerOneNum *= playground.Circuits[index].CountBoxes;
        }

        AnswerOne = AnswerOneNum.ToString();

        while(playground.Circuits.Count > 1)
        {
            playground.ConnectClosestPair();
        }

        AnswerTwo = ((long)playground.LastPair[0].x * (long)playground.LastPair[1].x).ToString();
    }
}
