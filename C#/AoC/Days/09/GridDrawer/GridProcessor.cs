using System.Data.Common;
using System.Drawing;

namespace AoC.Days;

internal class GridProcessor : IGridProcessor
{
    public GridColumn[] Grid { get; }

    public GridProcessor(List<Point> pointList)
    {
        Point gridCorner = GetGridCorner(pointList);
        int width = gridCorner.X + 1;
        int height = gridCorner.Y + 1;

        Grid = new GridColumn[width];
        for (int columnIndex = 0; columnIndex < width; columnIndex++)
        {
            Grid[columnIndex] = new GridColumn(height);
        }

        Draw(pointList);
    }

    private void Draw(List<Point> pointList)
    {
        DrawPoints(pointList);
        FillPass();
    }

    private void DrawPoints(List<Point> pointList)
    {
        for (int pointIndex = 0; pointIndex < pointList.Count;  pointIndex++)
        {
            int nextPointIndex = pointIndex == pointList.Count - 1 ? 0 : pointIndex + 1;
            Point point = pointList[pointIndex];
            Point nextPoint = pointList[nextPointIndex];

            DrawConnectingLine(point, nextPoint);
        }
    }

    private void DrawConnectingLine(Point startPoint, Point endPoint)
    {
        Grid[endPoint.X][endPoint.Y] = GridTile.Red;
        if (startPoint.X == endPoint.X)
        {
            int column = startPoint.X;
            int startRowIndex = Math.Min(startPoint.Y, endPoint.Y) + 1;
            int endRowIndex = Math.Max(startPoint.Y, endPoint.Y) - 1;
            foreach (int row in Enumerable.Range(startRowIndex, endRowIndex - startRowIndex + 1))
            {
                Grid[column][row] = GridTile.Green;
            }
        }
        else
        {
            int row = startPoint.Y;
            int startColumnIndex = Math.Min(startPoint.X, endPoint.X) + 1;
            int endColumnIndex = Math.Max(startPoint.X, endPoint.X) - 1;
            foreach (int column in Enumerable.Range(startColumnIndex, endColumnIndex - startColumnIndex + 1))
            {
                Grid[column][row] = GridTile.Green;
            }
        }
    }

    private void FillPass()
    {
        for (int Y = 0; Y < Grid[0].Count(); Y++)
        {
            int firstGreen = -1;
            int lastGreen = -1;
            for (int X = 0; X < Grid.Count(); X++)
            {
                if (Grid[X][Y] == GridTile.Green)
                {
                    if (firstGreen == -1) { firstGreen = X; }
                    lastGreen = X;
                }
            }

            if (firstGreen == -1) { continue; }
            
            for (int X = firstGreen; X <= lastGreen; X++)
            {
                if (Grid[X][Y] == GridTile.Black) { Grid[X][Y] = GridTile.Green; }
            }       
        }
    }

    private Point GetGridCorner(List<Point> pointList)
    {
        int xMax = 0;
        int yMax = 0;

        foreach (Point point in pointList)
        {
            if (point.X > xMax) { xMax = point.X; }
            if (point.Y > yMax) { yMax = point.Y; }
        }
        return new Point(xMax, yMax);
    }
}
