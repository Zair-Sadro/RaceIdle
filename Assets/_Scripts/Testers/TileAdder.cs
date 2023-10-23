using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TileAdder : MonoBehaviour
{
  public TileSetter _tileSetter;
  public TileType type;
    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(AddTile);
    }
    public void AddTile()
  {
        _tileSetter.TryAddTile(type);
        _tileSetter.TryAddTile(type);
        _tileSetter.TryAddTile(type);
    }
}
