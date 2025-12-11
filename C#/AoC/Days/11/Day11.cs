using AoC.ConsoleLogic;

namespace AoC.Days;

internal class Day11: Day
{
    protected override string[] FileInput(string filename)
    {
        FileTimer.Start();
        string path = Path.Combine(AppContext.BaseDirectory, "Data", filename);
        List<string> input = File.ReadAllLines(path).ToList();
        

        string runIndicator;
        if (filename == "day11testinput.txt")
        {
            int inputLength = input.Count; 
            runIndicator = "test " + inputLength;
            string path2 = Path.Combine(AppContext.BaseDirectory, "Data", "day11testinput2.txt");
            List<string> input2 = File.ReadAllLines(path2).ToList();
            input.Insert(0, runIndicator);
            input.AddRange(input2);
        }
        else
        {
            runIndicator = "real";
            input.Insert(0, runIndicator);
        }

        string[] indicatedInput = input.ToArray();
        FileTimer.Stop();

        return indicatedInput;
    }

    protected override void RunLogic(string[] input)
    {
        // process into two files.
        string indicatorLine = input[0];
        string[] firstInput = [];
        string[] secondInput = [];
        if (input[0].Length > 4)
        {
            int firstInputLastLine = int.Parse(input[0].Substring(5));
            firstInput = new string[firstInputLastLine];
            for (int lineIndex = 1; lineIndex <= firstInputLastLine; lineIndex++)
            {
                firstInput[lineIndex - 1] = input[lineIndex];
            }

            secondInput = new string[(input.Length - firstInputLastLine) - 1];
            for (int lineIndex = firstInputLastLine + 1; lineIndex < input.Length; lineIndex++)
            {
                secondInput[(lineIndex - firstInputLastLine) - 1] = input[lineIndex];
            }
        }
        else
        {
            firstInput = new string[input.Length - 1];
            for (int lineIndex = 1; lineIndex < input.Length; lineIndex++)
            {
                firstInput[lineIndex - 1] = input[lineIndex];
            }
            secondInput = firstInput;
        }

        // call internal run logic on the two files.
        RunLogicInternal(firstInput, secondInput);
    }

    protected void RunLogicInternal(string[] lines1, string[] lines2)
    {
        IDevicePather pather = new DevicePather(lines1);
        AnswerOne = pather.CountPaths("you", "out").ToString();

        IDevicePather[] pathers = new DevicePather[6];
        pather = new DevicePather(lines2);
        for (int patherIndex = 0; patherIndex < pathers.Length; patherIndex++)
        {
            pathers[patherIndex] = pather.Clone();
        }

        string[] startPoints = ["svr", "dac", "fft", "svr", "fft", "dac"];
        string[] endPoints = ["dac", "fft", "out", "fft", "dac", "out"];
        long[] results = new long[6];
        Parallel.For(0, pathers.Length, pathIndex =>
        {
            results[pathIndex] = pathers[pathIndex].CountPathsSafe(startPoints[pathIndex], endPoints[pathIndex]);
        });

        AnswerTwo = ((results[0] * results[1] * results[2]) +
                    (results[3] * results[4] * results[5]))
                    .ToString();
        
    }
}
