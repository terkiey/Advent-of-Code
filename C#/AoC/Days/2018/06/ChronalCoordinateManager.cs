using System.Drawing;

namespace AoC.Days;

internal class ChronalCoordinateManager
{
    // General Data
    private HashSet<Point> DestinationPointSet = [];

    // For Part One (define playable area)
    private int maxX = int.MinValue;
    private int minX = int.MaxValue;
    private int maxY = int.MinValue;
    private int minY = int.MaxValue;

    // For Part Two (domain parameter)
    private const int sumCheck = 10000;
    


    public void ParseData(string[] destinationCoordinatesArray)
    {
        foreach (string destinationCoordinates in destinationCoordinatesArray)
        {
            string[] coordsStringSplit = destinationCoordinates.Split(", ");
            int x = int.Parse(coordsStringSplit[0]);
            int y = int.Parse(coordsStringSplit[1]);
            Point coordPoint = new(x, y);
            DestinationPointSet.Add(coordPoint);

            if (x > maxX) maxX = x;
            if (x < minX) minX = x;
            if (y > maxY) maxY = y;
            if (y < minY) minY = y;
        }
    }

    // A point is a 'boundary point' if its finite area touches the 'playable area' boundary.
    public int LargestNonInfiniteArea()
    {
        Dictionary<Point, int> finiteAreaSizes = [];
        foreach (Point destination in DestinationPointSet)
        {
            finiteAreaSizes[destination] = 0;
        }

        HashSet<Point> boundaryDestinations = [];
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Point point = new(x, y);
                // Find closest destination, assign the point to it.
                int minDistance = int.MaxValue;
                Point destOwner = DestinationPointSet.First();
                foreach (Point destination in DestinationPointSet)
                {
                    int distance = Math.Abs(point.X - destination.X) + Math.Abs(point.Y - destination.Y);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        destOwner = destination;
                    }
                }
                finiteAreaSizes[destOwner]++;
                if (x == minX || x == maxX || y == minY || y == maxY)
                {
                    boundaryDestinations.Add(destOwner);
                }
            }
        }

        int maxFiniteArea = int.MinValue;
        foreach (var kvp in finiteAreaSizes)
        {
            if (!boundaryDestinations.Contains(kvp.Key))
            {
                maxFiniteArea = kvp.Value > maxFiniteArea ? kvp.Value : maxFiniteArea;
            }
        }
        return maxFiniteArea;
    }

    // Just making the assumption that they didnt set the sum check too high that the region spills out of the 'playable area', which held for my input.
    public int DistanceRegion()
    {
        HashSet<Point> regionPoints = [];
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Point point = new(x, y);
                int distanceSum = 0;
                foreach (Point destination in DestinationPointSet)
                {
                    distanceSum += Math.Abs(point.X - destination.X) + Math.Abs(point.Y - destination.Y);
                    if (distanceSum >= sumCheck)
                    {
                        break;
                    }
                }

                if (distanceSum < sumCheck)
                {
                    regionPoints.Add(point);
                }
            }
        }

        return regionPoints.Count;
    }

    private IEnumerable<Point> GetNeighbours(Point point)
    {
        List<Point> moves = [new(1, 0), new(-1, 0), new(0, 1), new(0, -1)];
        foreach (var move in moves)
        {
            yield return new Point(move.X + point.X, move.Y + point.Y);
        }
    }
}
