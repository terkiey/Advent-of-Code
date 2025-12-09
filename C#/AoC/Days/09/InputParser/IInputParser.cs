using System.Drawing;

namespace AoC.Days;

internal interface IInputParser
{
    List<Point> Parse(string[] input);
}
