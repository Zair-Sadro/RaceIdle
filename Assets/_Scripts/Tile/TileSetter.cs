using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class TileSetter : MonoBehaviour,ISaveLoad<TileSetterData>
{
    [SerializeField] private TileSetterData _data;
    [SerializeField] private Transform tilesSpawnerParent;
    
    [Header("Add to Unit Settings")]
    [SerializeField] private Transform setupPoint;
    [SerializeField] private int maxTiles;
    [SerializeField] private float powerTileJump=2f;
    [Space]
    [SerializeField,Range(0.1f,1f)] private float timeToRemoveTile;
    [SerializeField] private float delayToRemoveTile=0.7f;

    [Inject] private ResourceTilesSpawn _resourceTilesSpawn;

    [HideInInspector] public bool _isGivingTiles;


    [SerializeField]
    private List<Tile> _colectedTiles = new List<Tile>();
    private TileList _junkTiles = new(TileType.Junk);
    private TileList _ironTiles = new(TileType.Iron);
    private TileList _plasticTiles = new(TileType.Plastic);
    private TileList _rubberTiles = new(TileType.Rubber);

    private  Dictionary<TileType, TileList> tilesListsByType = new(4); 
    public IReadOnlyDictionary<TileType, TileList> TilesListsByType => tilesListsByType;



    private bool maxCapacity;

    public event Action<int> OnTilesCountChanged;
    public event Action<bool> OnTilesMaxCapacity;


    public List<Tile> Tiles => _colectedTiles;

    private float _timeToRemove;

    private void Awake()
    {
        _timeToRemove = timeToRemoveTile * 0.8f;
        tilesListsByType[TileType.Junk] = _junkTiles;
        tilesListsByType[TileType.Iron] = _ironTiles;
        tilesListsByType[TileType.Plastic] = _plasticTiles;
        tilesListsByType[TileType.Rubber] = _rubberTiles;
    }

    private void Start()
    {
        OnTilesMaxCapacity += SetTilesColliderStatus;
        GameEventSystem.TileSold +=((type)=> RemoveTiles(type,Vector3.zero,null,true));
        GameEventSystem.TileBought += AddTile;
        GameEventSystem.SoldALl += RemoveAll;
    }


    public void AddTile(Tile tile)
    {

        tile.OnTake();
        tile.Jump(setupPoint.position, powerTileJump, () =>
        {
            tile.transform.SetParent(setupPoint);
            tile.transform.localRotation = Quaternion.Euler(0, 0, 0);
        });


        _colectedTiles.Add(tile);
        tilesListsByType[tile.Type].AddTile(tile);

        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
        if (MaxTilesCapacity())
        {
            OnTilesMaxCapacity.Invoke(false); // Отлючаем коллайдеры всех активных тайлов
        }
    }
    /// <summary>
    /// Used only from tile shop after buy
    /// </summary>
    /// <param name="type"></param>
    public void AddTile(TileType type)
    {

        var tile =_resourceTilesSpawn.GetTile(type);
        tile.OnTake();
        tile.transform.SetParent(setupPoint);

        tile.transform.localRotation = Quaternion.Euler(0, 0, 0);


        _colectedTiles.Add(tile);
        tilesListsByType[tile.Type].AddTile(tile);

        OnTilesCountChanged?.Invoke(_colectedTiles.Count);

        if (MaxTilesCapacity())
        {
            OnTilesMaxCapacity.Invoke(false); // Отлючаем коллайдеры всех активных тайлов
        }
    }

    public void RemoveTiles(TileType type,Vector3 tilesPlace,Action<Tile> interatorCall, bool needClear = false)
    {
        if (!_isGivingTiles)
            StartCoroutine(RemovingTile(type, tilesPlace, interatorCall, needClear));
    }

    public void StopRemovingTiles()
    {

        _isGivingTiles = false;

    }


    public bool MaxTilesCapacity()
    {
        maxCapacity = _colectedTiles.Count == maxTiles;
        return maxCapacity;

    }


    private IEnumerator RemovingTile(TileType type, Vector3 tilesPlace,Action<Tile> interatorCall,bool needClear)
    {

        _isGivingTiles = true;

        TileList tiles = tilesListsByType[type];
        if (tiles.Count < 1) yield break;

        yield return new WaitForSeconds(delayToRemoveTile);

        for (int i = tiles.Count-1; i >= 0; i--)
        {
            tiles[i].ThrowTo(tilesPlace, timeToRemoveTile);
            interatorCall?.Invoke(tiles[i]);
            yield return new WaitForSeconds(_timeToRemove);

            if (needClear)
                ClearTiles(tiles[i]);
            else
                RemoveFromList(tiles[i]);

            if (_isGivingTiles == false)
                yield break;
        }
        _isGivingTiles = false;
    }

    private void RemoveFromList(Tile tile )
    {
        tilesListsByType[tile.Type].RemoveTile(tile);
        _colectedTiles.Remove(tile);


        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }

    private void ClearTiles(Tile tile)
    {
        tile.gameObject.SetActive(false);
        tile.OnGround();
        tile.transform.SetParent(tilesSpawnerParent);

        tilesListsByType[tile.Type].RemoveTile(tile);
        _colectedTiles.Remove(tile);


        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }

    private void SetTilesColliderStatus(bool value)
    {
        var tiles = _resourceTilesSpawn.GetAllActiveTiles();
        foreach (var tile in tiles)
        {
            tile.SetColliderActive(value);

        }
    }
    private void RemoveAll()
    {
        for (int i = 0; i < _colectedTiles.Count; i++)
        {
            ClearTiles(_colectedTiles[i]);

        }
        _colectedTiles.Clear();
    }


    public TileSetterData GetData()                                             
    {
        return _data;
    }

    public void Initialize(TileSetterData data)
    {
        _colectedTiles = data._colectedTiles;
        _ironTiles = data._ironTiles;
        _junkTiles = data._junkTiles;
        _rubberTiles = data._rubberTiles;
        _plasticTiles = data._plasticTiles;
    }
}

[Serializable]
public class TileSetterData
{

    public TileList _colectedTiles;
    public TileList _junkTiles;
    public TileList _ironTiles;
    public TileList _plasticTiles;
    public TileList _rubberTiles;

}


