using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TileAdder : MonoBehaviour
{
  public TileSetter _tileSetter;
    public WalletSystem wallet;
  public TileType type;
    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(AddTile);
    }
    public void AddTile()
    {
        if (type == TileType.Gold)
        {
            wallet.Income(1000f);
            return;
        }
            

        _tileSetter.TryAddTile(type);
        _tileSetter.TryAddTile(type);
        _tileSetter.TryAddTile(type);
    }
}
