using System.Drawing;

namespace AoC.Days;

internal interface IRectangleProcessor
{
    PriorityQueue<List<Point>, long> _rectangleMaxAreaHeap { get; }
    void CalculateRectangleHeap(List<Point> pointList);
    long CalculateArea(Point cornerOne, Point cornerTwo);
    public bool ValidateColors(Point pointOne, Point pointTwo, GridColumn[] grid);
}
