using System.Collections.Generic;

public class TileList : List<Tile>
{
    private TileType _type;

    public event System.Action<TileListInfo> OnTileAdded, OnTileRemoved;

    public TileType Type => _type;

    public TileList(TileType type)
    {
        _type = type;
    }
    public int TilesCount => this.Count;


    public void AddTile(Tile t)
    {

        this.Add(t);
        OnTileAdded?.Invoke(new TileListInfo(this));
    }
    public void RemoveTile(Tile t)
    {

        this.Remove(t);
        OnTileRemoved?.Invoke(new TileListInfo(this));
    }
}
public class TileListInfo
{
    private TileList _list;
    public int Count => _list.TilesCount;
    public TileType ListType => _list.Type;
    public TileListInfo(TileList list)
    {
        _list = list;
       
    }
}

