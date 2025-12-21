namespace AoC.Days;

internal class Y2015Day24 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        SleighBalancer balancer = new(inputLines);
        AnswerOne = balancer.GetIdealBalanceQEThreeWays().ToString();
        AnswerTwo = balancer.GetIdealBalanceQEFourWays().ToString();
    }
}
