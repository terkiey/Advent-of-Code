using System.Drawing;

namespace AoC.Days;

internal class RectangleProcessor : IRectangleProcessor
{
    public PriorityQueue<List<Point>, long> _rectangleMaxAreaHeap { get; } = new();

    public RectangleProcessor() { }

    public void CalculateRectangleHeap(List<Point> pointList)
    {
        for (int pointOneIndex = 0;  pointOneIndex < pointList.Count - 1; pointOneIndex++)
        {
            Point pointOne = pointList[pointOneIndex];
            for (int pointTwoIndex = pointOneIndex + 1; pointTwoIndex < pointList.Count; pointTwoIndex++)
            {
                Point pointTwo = pointList[pointTwoIndex];

                long area = CalculateArea(pointOne, pointTwo);
                List<Point> rectangle = [];
                rectangle.Add(pointOne);
                rectangle.Add(pointTwo);
                _rectangleMaxAreaHeap.Enqueue(rectangle, area * -1);
            }
        }
    }

    public long CalculateArea(Point pointOne, Point pointTwo)
    {
        long Width = Math.Abs(pointOne.X - pointTwo.X) + 1;
        long Height = Math.Abs(pointOne.Y - pointTwo.Y) + 1;

        return Width * Height;
    }

    public bool ValidateColors(Point pointOne, Point pointTwo, GridColumn[] grid)
    {
        return ValidateEdge(pointOne, pointTwo, grid);
    }

    private bool ValidateEdge(Point pointOne, Point pointTwo, GridColumn[] grid)
    {
        int xMin = Math.Min(pointOne.X, pointTwo.X);
        int xMax = Math.Max(pointOne.X, pointTwo.X);
        int yMin = Math.Min(pointOne.Y, pointTwo.Y);
        int yMax = Math.Max(pointOne.Y, pointTwo.Y);

        foreach (int X in Enumerable.Range(xMin, xMax - xMin + 1))
        {
            if (grid[X][yMin] == GridTile.Black) { return false; }
            if (grid[X][yMax] == GridTile.Black) { return false; }
        }

        foreach (int Y in Enumerable.Range(yMin, yMax - yMin + 1))
        {
            if (grid[xMin][Y] == GridTile.Black) { return false; }
            if (grid[xMax][Y] == GridTile.Black) { return false; }
        }

        return true;
    }

    private bool ValidateColorsBrute(Point pointOne, Point pointTwo, GridColumn[] grid)
    {
        int xMin = Math.Min(pointOne.X, pointTwo.X);
        int xMax = Math.Max(pointOne.X, pointTwo.X);
        int yMin = Math.Min(pointOne.Y, pointTwo.Y);
        int yMax = Math.Max(pointOne.Y, pointTwo.Y);

        for (int X = xMin; X <= xMax; X++)
        {
            for (int Y = yMin; Y <= yMax; Y++)
            {
                if (grid[X][Y] == GridTile.Black)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
