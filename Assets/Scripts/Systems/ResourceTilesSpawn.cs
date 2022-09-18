using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

//CanBeInjected//
public class ResourceTilesSpawn : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefub;
    [SerializeField] private int maxTilesAmount;

    [SerializeField] private Transform _resorceTilesParent;

    [Inject] private TileSetter _tileSetter;

    private ObjectPooler<Tile> _tilesPool;

    private void OnEnable()
    {
        _tilesPool = new ObjectPooler<Tile>(_tilePrefub, _resorceTilesParent);
        _tilesPool.CreatePool(maxTilesAmount);
    }

    public virtual List<Tile> GetRandomTiles(int min, int max)
    {
        List<Tile> newList = new List<Tile>();
        int randomAmount = Random.Range(min, max);

        for (int i = 0; i < randomAmount; i++)
            newList.Add(_tilesPool.GetFreeObject());

        return newList;
    }
    public Tile GetTile()
    {
        var tile = _tilesPool.GetFreeObject();

        if (_tileSetter.MaxTilesCapacity())
        {
            tile.SetColliderActive(false);
        }
        else
        {
            tile.SetColliderActive(true);
        }

        return tile;
    }
    public List<Tile> GetAllActiveTiles()
    {
       return _tilesPool.GetAllActiveObjects();
    }
}
