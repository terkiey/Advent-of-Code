namespace AoC.Days;

internal class Y2018Day01 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        int frequency = 0;
        foreach (string line in inputLines)
        {
            frequency += int.Parse(line);
        }
        AnswerOne = frequency.ToString();
        frequency = 0;
        HashSet<int> visited = [0];
        bool repeated = false;
        while (!repeated)
        {
            foreach(string line in inputLines)
            {
                frequency += int.Parse(line);
                if (!visited.Add(frequency))
                {
                    AnswerTwo = frequency.ToString();
                    repeated = true;
                    break;
                }
            }
        }
    }
}
