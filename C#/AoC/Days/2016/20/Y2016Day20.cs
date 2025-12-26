namespace AoC.Days;

internal class Y2016Day20 : Day
{
    protected override void RunLogic(string[] inputLines)
    {

        FirewallTranslator firewall = new();
        firewall.InputRanges(inputLines);
        AnswerOne = firewall.LowestValidIp().ToString();

        uint maxIpVal;
        switch (RunParameter)
        {
            case 1:
                maxIpVal = uint.MaxValue;
                break;

            case 2:
                maxIpVal = 9;
                break;

            default:
                maxIpVal = 0;
                break;
        }
        AnswerTwo = firewall.TotalAllowedIps(maxIpVal).ToString();
    }
}
