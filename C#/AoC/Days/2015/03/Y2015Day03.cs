namespace AoC.Days;

internal class Y2015Day03 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        HashSet<Position2D> cacheHousesHit = [];
        int[] santaPosition = [0, 0];
        string input = inputLines[0];
        int uniqueHouses = 0;
        foreach (char move in input)
        {
            switch (move)
            {
                case '>':
                    santaPosition[0]++;
                    break;

                case 'v':
                    santaPosition[1]--;
                    break;

                case '<':
                    santaPosition[0]--;
                    break;

                case '^':
                    santaPosition[1]++;
                    break;

                default:
                    throw new Exception("Only expecting < > ^ v");
            }

            if ( cacheHousesHit.Add( new(santaPosition[0], santaPosition[1]) ) )
            {
                uniqueHouses++;
            }
        }
        AnswerOne = uniqueHouses.ToString();

        cacheHousesHit = [];
        santaPosition = [0, 0];
        int[] roboSantaPosition = [0, 0];
        uniqueHouses = 0;
        bool moveParity = true;
        foreach (char move in input)
        {
            switch (move)
            {
                case '>':
                    if (moveParity)
                    {
                        santaPosition[0]++;
                    }
                    else
                    {
                        roboSantaPosition[0]++;
                    }
                    break;

                case 'v':
                    if (moveParity)
                    {
                        santaPosition[1]--;
                    }
                    else
                    {
                        roboSantaPosition[1]--;
                    }
                    break;

                case '<':
                    if (moveParity)
                    {
                        santaPosition[0]--;
                    }
                    else
                    {
                        roboSantaPosition[0]--;
                    }
                    break;

                case '^':
                    if (moveParity)
                    {
                        santaPosition[1]++;
                    }
                    else
                    {
                        roboSantaPosition[1]++;
                    }
                    break;

                default:
                    throw new Exception("Only expecting < > ^ v");
            }

            if (moveParity)
            {
                if (cacheHousesHit.Add(new(santaPosition[0], santaPosition[1])))
                {
                    uniqueHouses++;
                }
            }
            else
            {
                if (cacheHousesHit.Add(new(roboSantaPosition[0], roboSantaPosition[1])))
                {
                    uniqueHouses++;
                }
            }
            moveParity = !moveParity;
        }
        AnswerTwo = uniqueHouses.ToString();
    }
}
