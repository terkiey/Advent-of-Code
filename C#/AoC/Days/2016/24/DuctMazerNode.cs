using System.Drawing;

namespace AoC.Days;

internal class DuctMazerNode
{
    public Point Position;
    public int StepsTaken;
    public Point LastMove;

    public DuctMazerNode (Point location, int currentSteps)
    {
        Position = location;
        StepsTaken = currentSteps;
    }

    public void Move(Point moveOffset)
    {
        Position.Offset(moveOffset);
        LastMove = moveOffset;
        StepsTaken++;
    }
}
