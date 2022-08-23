using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CanBeInjected//
public class ResourceTilesSpawn : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefub;
    [SerializeField] private int maxTilesAmount;

    [SerializeField] private Transform _resorceTilesParent;

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
        return _tilesPool.GetFreeObject();
    }
    public List<Tile> GetAllActiveTiles()
    {
       return _tilesPool.GetAllActiveObjects();
    }
}
