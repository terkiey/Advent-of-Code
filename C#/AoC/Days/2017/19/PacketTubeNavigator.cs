using System.Drawing;
using System.Text;

namespace AoC.Days;

internal class PacketTubeNavigator
{
    private Point currentPosition { get; set; } = new Point();
    private Point currentDirection { get; set; } = new(0, 1);
    private string[] maze = [];

    // Part One
    private StringBuilder LocationTracker { get; } = new();
    public string LocationsOrder => LocationTracker.ToString();

    // Part Two
    private int stepCounter { get; set; } = 1;
    public string StepsRequired => stepCounter.ToString();

    public void Navigate(string[] inputLines)
    {
        SetStartPoint(inputLines[0]);
        maze = inputLines;
        char currentChar = GetPositionChar(currentPosition);
        while (true)
        {
            // If you exceeded the end of the line.
            if (currentChar == ' ')
            {
                stepCounter--;
                break;
            }

            // Move based on calculated move direction.
            Move();

            currentChar = GetPositionChar(currentPosition);
            // Add location if stood on one.
            if (char.IsAsciiLetterUpper(currentChar))
            {
                LocationTracker.Append(currentChar);
            }

            // Calculate next move direction
            if (currentChar == '+')
            {
                // Try forwards
                Point newPosition = new(currentPosition.X + currentDirection.X, currentPosition.Y + currentDirection.Y);
                if (IsInBounds(newPosition) && GetPositionChar(newPosition) != ' ')
                {
                    continue;
                }

                // Try turning right
                Point rightTurn = RotateMove90Clockwise(currentDirection);
                newPosition = new(currentPosition.X + rightTurn.X, currentPosition.Y + rightTurn.Y);
                if (IsInBounds(newPosition) && GetPositionChar(newPosition) != ' ')
                {
                    currentDirection = rightTurn;
                    continue;
                }

                // else turn left
                Point leftTurn = RotateMove90AntiClockwise(currentDirection);
                currentDirection = leftTurn;
            }

        }
    }

    private void SetStartPoint(string topLine)
    {
        for (int x = 0; x < topLine.Length; x++)
        {
            if (topLine[x] == '|')
            {
                currentPosition = new Point(x, 0);
            }
        }
    }

    private char GetPositionChar(Point position)
    {
        return maze[position.Y][position.X];
    }

    private Point RotateMove90Clockwise(Point move)
    {
        return new(move.Y, move.X * -1);
    }

    private Point RotateMove90AntiClockwise(Point move)
    {
        return new(move.Y * -1, move.X);
    }

    private void Move()
    {
        currentPosition = new(currentPosition.X + currentDirection.X, currentPosition.Y + currentDirection.Y);
        stepCounter++;
    }

    private bool IsInBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < maze[0].Length && position.Y < maze.Length;
    }
}
