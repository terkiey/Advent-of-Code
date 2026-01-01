namespace AoC.Days;

internal class Y2017Day05 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        int[] jumpArray = JumpArray(inputLines);
        int pointer = 0;
        int stepCount = 0;
        while (pointer < inputLines.Length && pointer > -1)
        {
            pointer += jumpArray[pointer]++;
            stepCount++;
        }
        AnswerOne = stepCount.ToString();

        jumpArray = JumpArray(inputLines);
        pointer = 0;
        stepCount = 0;
        while (pointer < inputLines.Length && pointer > -1)
        {
            if (jumpArray[pointer] < 3)
            {
                pointer += jumpArray[pointer]++;
            }
            else
            {
                pointer += jumpArray[pointer]--;
            }
            stepCount++;
        }
        AnswerTwo = stepCount.ToString();
    }

    private int[] JumpArray(string[] jumpStrings)
    {
        int[] jumpArray = new int[jumpStrings.Length];
        int pointer = 0;
        foreach (string jumpString in jumpStrings)
        {
            jumpArray[pointer++] = int.Parse(jumpString);
        }
        return jumpArray;
    }
}
