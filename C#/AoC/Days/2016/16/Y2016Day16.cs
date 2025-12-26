using System.ComponentModel.DataAnnotations;

namespace AoC.Days;

internal class Y2016Day16 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        DragonChecksumCalculator calculator = new();
        AnswerOne = calculator.DragonExtendAndChecksum(inputLines[0], 272);
        AnswerTwo = calculator.DragonExtendAndChecksum(inputLines[0], 35651584);
    }
}
