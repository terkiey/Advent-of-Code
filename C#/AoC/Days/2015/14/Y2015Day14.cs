namespace AoC.Days;

internal class Y2015Day14 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        int raceTime = -1;
        if (RunParameter == 1)
        {
            raceTime = 2503;
        }
        else
        {
            raceTime = 1000;
        }

            int[][] reindeers = new int[inputLines.Length][];
        for (int i = 0; i < inputLines.Length; i++)
        {
            reindeers[i] = new int[3];
        }

        for (int lineIndex = 0;  lineIndex < inputLines.Length; lineIndex++)
        {
            string[] splitLine = inputLines[lineIndex].Split(' ');
            reindeers[lineIndex][0] = int.Parse(splitLine[3]);
            reindeers[lineIndex][1] = int.Parse(splitLine[6]);
            reindeers[lineIndex][2] = int.Parse(splitLine[13]);
        }

        int winningDistance = 0;
        foreach (int[] reindeer in reindeers)
        {
            int speed = reindeer[0];
            int flyTime = reindeer[1];
            int restTime = reindeer[2];

            int reindeerDistance = Race(speed, flyTime, restTime, raceTime);
            winningDistance = reindeerDistance > winningDistance ? reindeerDistance : winningDistance;
        }
        AnswerOne = winningDistance.ToString();

        int[] stateTimes = new int[inputLines.Length];
        bool[] restingFlags = new bool[inputLines.Length];
        int[] distances = new int[inputLines.Length];
        int[] points = new int[inputLines.Length];

        for (int reindeerIndex = 0;  reindeerIndex < inputLines.Length; reindeerIndex++)
        {
            stateTimes[reindeerIndex] = reindeers[reindeerIndex][1];
        }
        for (int second = 0; second < raceTime; second++)
        {
            for (int reindeerIndex = 0; reindeerIndex < inputLines.Length; reindeerIndex++)
            {
                int[] reindeer = reindeers[reindeerIndex];
                int speed = reindeer[0];
                int flyTime = reindeer[1];
                int restTime = reindeer[2];

                stateTimes[reindeerIndex]--;
                switch (restingFlags[reindeerIndex])
                {
                    case false:
                        distances[reindeerIndex] += reindeers[reindeerIndex][0];
                        if(stateTimes[reindeerIndex] == 0)
                        {
                            restingFlags[reindeerIndex] = true;
                            stateTimes[reindeerIndex] = restTime;
                        }
                        break;

                    case true:
                        if (stateTimes[reindeerIndex] == 0)
                        {
                            restingFlags[reindeerIndex] = false;
                            stateTimes[reindeerIndex] = flyTime;
                        }
                        break;
                }
            }

            int furthestDistance = distances.Max();
            for (int reindeerIndex = 0; reindeerIndex < inputLines.Length; reindeerIndex++)
            {
                if (distances[reindeerIndex] == furthestDistance)
                {
                    points[reindeerIndex]++;
                }
            }
        }

        AnswerTwo = points.Max().ToString();
    }

    private int Race(int speed, int flyTime, int restTime, int raceTime)
    {
        int currentTime = 0;
        int distance = 0;
        bool resting = false;
        while (currentTime < raceTime)
        {
            switch (resting)
            {
                case false:
                    currentTime += flyTime;
                    distance += flyTime * speed;
                    resting = true;
                    break;

                case true:
                    currentTime += restTime;
                    resting = false;
                    break;
            }

            if (currentTime > raceTime && resting == true)
            {
                int excessTime = currentTime - raceTime;
                distance -= excessTime * speed;
            }
        }
        return distance;
    }
}
