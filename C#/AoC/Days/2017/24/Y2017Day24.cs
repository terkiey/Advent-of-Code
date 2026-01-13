namespace AoC.Days;

internal class Y2017Day24 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        MagneticComponentBridgeBuilder bridger = new();
        bridger.ParseComponents(inputLines);
        AnswerOne = bridger.FindStrongestBridgeStrength().ToString();
        AnswerTwo = bridger.FindLongestStrongestBridgeStrength().ToString();
    }
}
// TODO_MID: Turn this into a graph problem and solve the maximum length path instead of BFS-ing it. Currently the runtime is just under 5 seconds. 
