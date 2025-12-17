namespace AoC.Days;

internal class Y2015Day18 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        AnimatedLightBoard lightBoard = new AnimatedLightBoard(inputLines);
        lightBoard.Step(100);
        int onLights = lightBoard.CountLightsOn();
        AnswerOne = onLights.ToString();

        lightBoard = new AnimatedLightBoard(inputLines);
        lightBoard.StepPartTwo(100);
        onLights = lightBoard.CountLightsOn();
        AnswerTwo = onLights.ToString();
    }
}
