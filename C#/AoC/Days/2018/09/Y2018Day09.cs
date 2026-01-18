namespace AoC.Days;

internal class Y2018Day09 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        MarbleCircleGameSimulator simulator = new();
        int playerCount = int.Parse(inputLines[0].Split(' ')[0]);
        int lastMarble = int.Parse(inputLines[0].Split(' ')[^2]);
        AnswerOne = simulator.GetWinningScore(playerCount, lastMarble).ToString();
        //TODO_HIGH: This is very slow, and insertion/deletion is way faster for doubly linked lists, so implement like that.
        // (I could probably figure out a maths equation for the answer too, but doubly linked list is the programming solution)
        AnswerTwo = simulator.GetWinningScore(playerCount, lastMarble * 100).ToString();
    }
}
