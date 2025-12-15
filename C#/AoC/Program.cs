using AoC.ConsoleLogic;
using Microsoft.VisualBasic;

namespace AoC;

internal class Program
{
    static void Main(string[] args)
    {
        int year;
        if (args.Length == 0)
        {
            year = 2025;
        }
        else
        {
            year = int.Parse(args[0]);
        }

        IUserInputProcessor userInputProcessor = new UserInputProcessor();
        IRunCommandProcessor runCommandProcessor = new RunCommandProcessor(year);

        while (true)
        {
            RunCommand command = userInputProcessor.AskForCommand();
            runCommandProcessor.Process(command);
        }
    }
}
