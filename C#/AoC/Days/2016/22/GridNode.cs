namespace AoC.Days;

internal class GridNode
{
    public int Used { get; set; }
    public int Available { get; set; }
    public int[] Location { get; set; }

    public GridNode(int used, int available, int[] location)
    {
        Used = used;
        Available = available;
        Location = location;
    }
}
