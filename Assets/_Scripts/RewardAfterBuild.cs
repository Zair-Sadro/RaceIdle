using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class RewardAfterBuild : MonoBehaviour, IBuildable
{
    [SerializeField] private TileSetter _tileSetter;
    [SerializeField] private GameObject _uniqCar;
    public void Build()
    {
        _uniqCar.SetActive(true);
        if (YandexGame.savesData.buildData.Buildings.Contains(3))
            return;

        _tileSetter.TryAddTile(TileType.Junk);
        _tileSetter.TryAddTile(TileType.Iron);
        _tileSetter.TryAddTile(TileType.Plastic);
        _tileSetter.TryAddTile(TileType.Rubber);
    }

  
}
