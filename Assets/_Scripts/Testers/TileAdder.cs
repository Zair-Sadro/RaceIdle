using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAdder : MonoBehaviour
{
  public TileSetter _tileSetter;
  public TileType type;
  public void AddTile()
  {
        _tileSetter.AddTile(type);
        _tileSetter.AddTile(type);
        _tileSetter.AddTile(type);
    }
}
