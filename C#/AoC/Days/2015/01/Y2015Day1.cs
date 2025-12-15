namespace AoC.Days;

internal class Y2015Day1 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        AnswerOne = (inputLines[0].Count(c => c == '(') - inputLines[0].Count(c => c == ')')).ToString();

        int basementEnteredPosition = -1;
        int currentPos = 0;
        for (int moveIndex = 0; moveIndex < inputLines[0].Length; moveIndex++) 
        {
            char move = inputLines[0][moveIndex];
            switch (move)
            {
                case '(':
                    currentPos++;
                    break;

                case ')':
                    currentPos--;
                    break;

                default:
                    throw new Exception("Only expecting ( and )");
            }

            if (currentPos < 0)
            {
                basementEnteredPosition = moveIndex + 1;
                break;
            }
        }

        AnswerTwo = basementEnteredPosition.ToString();
    }
}
