using System.Drawing;

namespace AoC.Days;

internal class Y2025Day9: Day
{
    protected override void RunLogic(string[] lines)
    {
        IInputParser inputParser = new InputParser();
        IRectangleProcessor rectangleProcessor = new RectangleProcessor();

        List<Point> pointList = inputParser.Parse(lines);
        rectangleProcessor.CalculateRectangleHeap(pointList);
        PriorityQueue<List<Point>, long> rectangleHeap = rectangleProcessor._rectangleMaxAreaHeap;
        List<Point> largestRectangle = rectangleHeap.Dequeue();
        Point cornerOne = largestRectangle[0];
        Point cornerTwo = largestRectangle[1];
        AnswerOne = rectangleProcessor.CalculateArea(cornerOne, cornerTwo).ToString();

        
        IGridProcessor gridProcessor = new GridProcessor(pointList);
        GridColumn[] grid = gridProcessor.Grid;
        if (rectangleProcessor.ValidateColors(cornerOne, cornerTwo, grid))
        {
            AnswerTwo = rectangleProcessor.CalculateArea(cornerOne, cornerTwo).ToString();
        }

        while (AnswerTwo == String.Empty)
        {
            List<Point> rectangle = rectangleHeap.Dequeue();
            cornerOne = rectangle[0];
            cornerTwo = rectangle[1];
            if (rectangleProcessor.ValidateColors(cornerOne,cornerTwo, grid))
            {
                AnswerTwo = rectangleProcessor.CalculateArea(cornerOne, cornerTwo).ToString();
            }
        }
    }
}
