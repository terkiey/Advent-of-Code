namespace AoC.Days;

internal class GridColumn
{
    public GridTile[] Tiles { get; }

    public GridColumn(int height)
    {
        Tiles = new GridTile[height];
    }

    public GridTile this[int index]
    {
        get { return Tiles[index]; }
        set { Tiles[index] = value; }
    }

    public int Count()
    {
        return Tiles.Count();
    }
}
