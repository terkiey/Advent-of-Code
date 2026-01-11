namespace AoC.Days;

internal class Y2017Day16 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        ProgramDancerPermutations dancer = new();
        dancer.Dance(inputLines[0]);
        AnswerOne = dancer.GetProgramOrder();

        dancer.DanceMultipleTimes(inputLines[0], 1000000000);
        AnswerTwo = dancer.GetProgramOrder();
    }
}
