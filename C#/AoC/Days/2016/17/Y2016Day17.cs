namespace AoC.Days;

internal class Y2016Day17 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        VaultMd5Mazer mazer = new();
        mazer.SetSeed(inputLines[0]);
        AnswerOne = mazer.FastestVaultPath();
        AnswerTwo = mazer.SlowestVaultPath().Length.ToString();
    }
}
