using System.Drawing;

namespace AoC.Days;

internal class SporificaVirusSimulator
{
    private Point CurrentDirection = new Point(0, 1);
    private Point CurrentPosition = new(0, 0);
    private Dictionary<Point, SporificaNodeStates> StatefulPositions = [];

    // AOC
    public int InfectingBurstCount { get; private set; } = 0;

    public void InitialiseState(string[] infectionMap)
    {
        InfectingBurstCount = 0;
        StatefulPositions.Clear();
        CurrentDirection = new Point(0, 1);
        int width = infectionMap.Length;
        int height = infectionMap[0].Length;
        for (int rowIndex = 0; rowIndex < height; rowIndex++)
        {
            string infectionRow = infectionMap[rowIndex];
            for (int colIndex = 0; colIndex < width; colIndex++)
            {
                if (infectionRow[colIndex] == '#')
                {
                    Point infectedPoint = new Point(colIndex, height - 1 - rowIndex);
                    StatefulPositions.Add(infectedPoint, SporificaNodeStates.Infected);
                }
            }
        }
        CurrentPosition = new(width / 2, height / 2);
    }

    public void RunXEvolvedBursts(int burstCount)
    {
        for (int burstIndex = 0; burstIndex < burstCount; burstIndex++)
        {
            if (!StatefulPositions.TryGetValue(CurrentPosition, out SporificaNodeStates state))
            {
                TurnLeft();
                StatefulPositions[CurrentPosition] = SporificaNodeStates.Weakened;
            }
            else switch (state)
            {
                case SporificaNodeStates.Weakened:
                    StatefulPositions[CurrentPosition] = SporificaNodeStates.Infected;
                    InfectingBurstCount++;
                    break;

                case SporificaNodeStates.Infected:
                    TurnRight();
                    StatefulPositions[CurrentPosition] = SporificaNodeStates.Flagged;
                    break;

                case SporificaNodeStates.Flagged:
                    TurnAround();
                    StatefulPositions.Remove(CurrentPosition);
                    break;
            }
            Move();
        }
    }

    public void RunXBursts(int burstCount)
    {
        for (int burstIndex = 0; burstIndex < burstCount; burstIndex++)
        {
            bool nodeInfected = StatefulPositions.ContainsKey(CurrentPosition);
            if (nodeInfected)
            {
                TurnRight();
                StatefulPositions.Remove(CurrentPosition);
            }
            else
            {
                TurnLeft();
                StatefulPositions[CurrentPosition] = SporificaNodeStates.Infected;
                InfectingBurstCount++;
            }
            Move();
        }
    }

    private void TurnAround() => CurrentDirection = new(CurrentDirection.X * -1, CurrentDirection.Y * -1);
    private void TurnRight() => CurrentDirection = new(CurrentDirection.Y, CurrentDirection.X * -1);
    private void TurnLeft() => CurrentDirection = new(CurrentDirection.Y * -1, CurrentDirection.X);
    private void Move() => CurrentPosition.Offset(CurrentDirection);
}
