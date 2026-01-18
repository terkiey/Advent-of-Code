namespace AoC.Days;

internal class Y2018Day07 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        StepDependencyCalculator calc = new();
        calc.Input(inputLines);
        AnswerOne = calc.PartOne();
        int workers = -1;
        int taskTime= -1;
        switch (RunParameter)
        {
            case 1:
                workers = 5;
                taskTime = 60;
                break;

            case 2:
                workers = 2;
                taskTime = 0;
                break;
        }
        AnswerTwo = calc.PartTwo(workers, taskTime).ToString();
    }
}
