using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTilesSpawn : MonoBehaviour
{
    [SerializeField] private Tile _junkPrefab;
    [SerializeField] private Tile _ironPrefab;
    [SerializeField] private Tile _plastPrefab;
    [SerializeField] private Tile _rubberPrefab;

    [SerializeField] private int maxTilesAmount;
    [SerializeField] private Transform _tilesParent;


    private ObjectPooler<Tile> _junkPool;
    private ObjectPooler<Tile> _ironPool;
    private ObjectPooler<Tile> _plastPool;
    private ObjectPooler<Tile> _rubberPool;

    private List<ObjectPooler<Tile>> _poolList = new();

    private TileSetter _tileSetter => InstantcesContainer.Instance.TileSetter;
    private string[] _tileTypesNames;

    void Start()
    {
        _tileTypesNames = Enum.GetNames(typeof(TileType));
    }
    private void OnEnable()
    {
        _junkPool = CreatePool(_junkPrefab);
        _ironPool = CreatePool(_ironPrefab);
        _plastPool = CreatePool(_plastPrefab);
        _rubberPool = CreatePool(_rubberPrefab);



        ObjectPooler<Tile> CreatePool(Tile pref)
        {
            var pool = new ObjectPooler<Tile>(pref, _tilesParent);
            pool.CreatePool(maxTilesAmount);
            _poolList.Add(pool);
            pool.AutoExpand = true;

            return pool;
        }

    }


    public Tile GetTile(TileType type)
    {
        var tile = PoolByType(type).GetFreeObject();
        tile.InjectTileSetter(_tileSetter);

        return tile;

    }
    public Tile GetRandomTile()
    {
        var tile = PoolByType(RandomTileType()).GetFreeObject();
        tile.InjectTileSetter(_tileSetter);

        return tile;
    }
    private TileType RandomTileType()
    {
        var r = UnityEngine.Random.Range(0, _tileTypesNames.Length);

        var name = _tileTypesNames[r];
        return (TileType)Enum.Parse(typeof(TileType), name);
    }
    public void ToPool(Tile t)
    {
        t.transform.parent = _tilesParent;
        t.gameObject.SetActive(false);
    }

    private ObjectPooler<Tile> PoolByType(TileType type)
    {
        switch (type)
        {
            case TileType.Junk:
                return _junkPool;

            case TileType.Iron:
                return _ironPool;

            case TileType.Rubber:
                return _rubberPool;

            case TileType.Plastic:
                return _plastPool;

        }
        return null;
    }

    //public virtual List<Tile> GetRandomTiles(int min, int max)
    //{
    //    List<Tile> newList = new List<Tile>();
    //    int randomAmount = Random.Range(min, max);

    //    for (int i = 0; i < randomAmount; i++)
    //        newList.Add(_tilesPool.GetFreeObject());

    //    return newList;
    //}
}
