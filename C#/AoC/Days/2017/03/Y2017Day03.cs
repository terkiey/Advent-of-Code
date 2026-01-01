using System.Drawing;
using System.Reflection;

namespace AoC.Days;

internal class Y2017Day03 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        // Notice that every odd square number is a diagonal line down-right going from 1.
        // Find the square numbers that surround the desired figure and then path from there.

        int target = int.Parse(inputLines[0]);
        int corner = 1;
        while (corner*corner <= target) { corner += 2; }

        // Path backwards from where you end up.
        Point gridPos = new((corner - 1) / 2, -1 * ((corner - 1) / 2));
        int edgeUsed = 0;
        Point currentMove = new(-1, 0);
        int pointer = corner * corner;
        
        while (true)
        {
            if (pointer == target) { break; }
            gridPos.Offset(currentMove);
            pointer--;
            edgeUsed++;
            if (edgeUsed == corner - 1)
            {
                currentMove = RotateClockwise90(currentMove);
                edgeUsed = 0;
            }
        }
        AnswerOne = (Math.Abs(gridPos.X) + Math.Abs(gridPos.Y)).ToString();

        gridPos = new(0, 0);
        Dictionary<Point, int> numbers = [];
        int gridSizeGuess = 100;
        for (int xCo = gridSizeGuess * -1; xCo <= gridSizeGuess; xCo++)
        {
            for (int yCo = gridSizeGuess * -1; yCo <= gridSizeGuess; yCo++)
            {
                Point point = new(xCo, yCo);
                numbers[point] = 0;
            }
        }
        numbers[gridPos] = 1;

        int turnCounter = 0;
        int moveCounter = 0;
        int sideLength = 1;
        currentMove = new(1, 0);
        while (numbers[gridPos] < target)
        {
            gridPos.Offset(currentMove);
            moveCounter++;
            if (moveCounter == sideLength)
            {
                currentMove = RotateAntiClockwise90(currentMove);
                turnCounter++;
                moveCounter = 0;
            }

            if (turnCounter == 2)
            {
                sideLength++;
                turnCounter = 0;
            }
            numbers[gridPos] = numbers[new(gridPos.X + 1, gridPos.Y)] +
                               numbers[new(gridPos.X + 1, gridPos.Y + 1)] + 
                               numbers[new(gridPos.X, gridPos.Y + 1)] + 
                               numbers[new(gridPos.X - 1, gridPos.Y + 1)] +
                               numbers[new(gridPos.X - 1, gridPos.Y)] + 
                               numbers[new(gridPos.X - 1, gridPos.Y - 1)] + 
                               numbers[new(gridPos.X, gridPos.Y - 1)] + 
                               numbers[new(gridPos.X + 1, gridPos.Y - 1)];
        }
        AnswerTwo = numbers[gridPos].ToString();
    }

    private Point RotateClockwise90(Point move)
    {
        int newX = move.Y;
        int newY = move.X * -1;
        return new(newX, newY);
    }

    private Point RotateAntiClockwise90(Point move)
    {
        int newX = move.Y * -1;
        int newY = move.X;
        return new Point(newX, newY);
    }
}
