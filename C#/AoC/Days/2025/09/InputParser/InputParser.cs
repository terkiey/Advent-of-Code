using System.Drawing;

namespace AoC.Days;

internal class InputParser : IInputParser
{
    public List<Point> Parse(string[] inputLines)
    {
        List<Point> pointList = [];
        foreach (string line in inputLines)
        {
            int[] coords = line.Split(',').Select(c => int.Parse(c)).ToArray();
            Point point = new Point(coords[0], coords[1]);
            pointList.Add(point);
        }

        return pointList;
    }
}
