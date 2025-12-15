namespace AoC.Days;

internal class JunctionBox
{
    public int x;
    public int y;
    public int z;

    public JunctionBox(int inX, int inY, int inZ)
    {
        y = inY;
        z = inZ;
        x = inX;
    }

    public double DistanceBetween(JunctionBox otherBox)
    {
        long xDiff = x - otherBox.x;
        long yDiff = y - otherBox.y;
        long zDiff = z - otherBox.z;

        return Math.Sqrt((xDiff * xDiff) + (yDiff * yDiff) + (zDiff * zDiff));
    }
}
