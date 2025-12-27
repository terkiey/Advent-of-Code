using System.ComponentModel;

namespace AoC.Days;

internal class Y2016Day21 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        char[] password = RunParameter switch
        {
            1 => ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'],
            2 => ['a', 'b', 'c', 'd', 'e'],
            _ => [],
        };
        PasswordScramblingComputer scrambler = new();
        AnswerOne = scrambler.ScrambleFromInstructions(password, inputLines);

        // Test case can be the answerOne answer...
        char[] scrambledPassword = [];
        if (RunParameter == 1)
        {
            scrambledPassword = ['f', 'b', 'g', 'd', 'c', 'e', 'a', 'h'];
        }
        else if (RunParameter == 2)
        {
            scrambledPassword = AnswerOne.ToCharArray();
        }
        AnswerTwo = scrambler.UnscrambleFromInstructions(scrambledPassword, inputLines);
    }
}
