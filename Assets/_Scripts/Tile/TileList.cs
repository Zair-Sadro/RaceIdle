using System;
using System.Collections.Generic;

[Serializable]
public class TileList : List<Tile>
{
    public event Action<TileType,int> OnTileAdded, OnTileRemoved;

    public TileType Type { get; }

    public TileList(TileType type)
    {
        Type = type;
    }
    public int TilesCount => this.Count;

    public void AddTile(Tile t)
    {

        this.Add(t);
        OnTileAdded?.Invoke(Type,this.Count);
    }
    public void RemoveTile(Tile t)
    {
        this.Remove(t);
        OnTileRemoved?.Invoke(Type,this.Count);
    }
}

