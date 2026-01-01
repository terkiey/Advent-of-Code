namespace AoC.Days;

internal class Y2017Day01 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        string captcha = inputLines[0];
        int captchaSum = 0;
        char last = captcha[^1];
        for (int digitIndex = 0; digitIndex < inputLines[0].Length; digitIndex++)
        {
            char current = captcha[digitIndex];
            captchaSum += current == last ? (int)char.GetNumericValue(current) : 0;
            last = current;
        }
        AnswerOne = captchaSum.ToString();

        captchaSum = 0;
        int captchaJump = captcha.Length / 2;
        for (int digitIndex = 0; digitIndex < captcha.Length; digitIndex++)
        {
            char current = captcha[digitIndex];
            int matchIndex = (digitIndex + captchaJump) % captcha.Length;
            captchaSum += current == captcha[matchIndex] ? (int)char.GetNumericValue(current) : 0;
        }
        AnswerTwo = captchaSum.ToString();
    }
}
