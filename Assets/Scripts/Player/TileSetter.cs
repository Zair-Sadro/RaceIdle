using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Events;
using Zenject;

[Serializable]
public class TileSetterData
{

    public List<Tile> _colectedTiles;
    public List<Tile> _junkTiles;
    public List<Tile> _ironTiles;

}

public class TileSetter : MonoBehaviour,ISaveLoad<TileSetterData>
{
    [SerializeField] private TileSetterData _data;

    [SerializeField] private Transform tilesSpawnerParent;
    
    [Header("Add to Unit Settings")]
    [SerializeField] private Transform setupPoint;
    [SerializeField] private Vector3 tilesScale;
    [SerializeField] private float tileSetSpeed;
    [SerializeField, Range(0, 100)] private float zOffset;
    [SerializeField, Range(0, 100)] private float yOffset = 0;
    [SerializeField] private int maxTiles;
    [SerializeField] private int tilesRow = 2;
    [Space]
    [SerializeField,Range(0.1f,1f)] private float timeToRemoveTile;

    [Inject] private ResourceTilesSpawn _resourceTilesSpawn;

    private float _currentRemovingTime;
    private bool _isInBenchZone;
    private bool _isGivingTiles;

    private BaseMachineTool _currentBench;

    [SerializeField]
    private List<Tile> _colectedTiles = new List<Tile>();
    private List<Tile> _junkTiles = new List<Tile>();
    private List<Tile> _ironTiles = new List<Tile>();
    private List<Tile> _givenTiles = new List<Tile>();

    private bool maxCapacity;

    public event Action<int> OnTilesCountChanged;
    public event Action<bool> OnTilesMaxCapacity;

    public event Action<MachineTool> OnBenchZoneEnter;
    public event Action OnBenchZoneExit;

    public List<Tile> Tiles => _colectedTiles;
    public List<Tile> GivenTiles => _givenTiles;


    private void Start()
    {
        _currentRemovingTime = timeToRemoveTile;
        OnTilesMaxCapacity += SetTilesColliderStatus;
    }


    private void AddTile(Tile tile)
    {
        tile.OnBack();       
        tile.transform.SetParent(setupPoint);

        tile.transform.localRotation = Quaternion.Euler(0, 0, 0);


        _colectedTiles.Add(tile);
        GetTileListByType(tile.Type)?.Add(tile);

        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }

    public void RemoveTiles(TileType type,Vector3 tilesPlace,Action interatorCall)
    {
        if (!_isGivingTiles)
            StartCoroutine(RemovingTile(type, tilesPlace, interatorCall));
    }

    private IEnumerator RemovingTile(TileType type, Vector3 tilesPlace,Action interatorCall)
    {

        _isGivingTiles = true;

        List<Tile> tiles =  GetTileListByType(type);
        if (tiles == null) yield break;

        for (int i = tiles.Count - 1; i >= 0; i--)
        {
            tiles[i].ThrowTo(tilesPlace, timeToRemoveTile);
            yield return new WaitForSeconds(timeToRemoveTile);
            interatorCall.Invoke();
            _givenTiles.Add(tiles[i]);

            ClearTiles(i, type);

            if (_isGivingTiles == false) yield break;
        }
        _isGivingTiles = false;
    }

    private void ClearTiles(int index,TileType type)
    {
        _colectedTiles[index].gameObject.SetActive(false);
        _colectedTiles[index].OnGround();
        _colectedTiles[index].transform.SetParent(tilesSpawnerParent);

        GetTileListByType(type).Remove(_colectedTiles[index]);
        _colectedTiles.Remove(_colectedTiles[index]);


        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }

    public void StopRemovingTiles()
    {
        _isGivingTiles = false;
        //StopAllCoroutines();
       // SortTiles();
    }

    private void SetTilesColliderStatus(bool value)
    {
        var tiles = _resourceTilesSpawn.GetAllActiveTiles();
        foreach (var tile in tiles)
        {
            tile.SetColliderActive(value);

        }
    }

    public bool MaxTilesCapacity()
    {
        maxCapacity = _colectedTiles.Count == maxTiles;
        return maxCapacity;

    }



    // œ≈–≈œ»—¿“‹ ƒÀﬂ Œœ“»Ã»«¿÷»» : œ”—“‹ TILE ¡”ƒ≈“ ‘» —»–Œ¬¿“‹ TRIGGERENTER PLAYER             //
    // » ◊≈–≈« »Õ∆≈ Õ”“€… TILESETTER ¬—≈ ƒ≈À¿≈“ ¡≈« ›“Œ√Œ TRYGETCOMPONENT                        //
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Tile tile) && _colectedTiles.Count < maxTiles)
        {
            AddTile(tile);

            if (MaxTilesCapacity())
                OnTilesMaxCapacity.Invoke(false); // ŒÚÎ˛˜‡ÂÏ ÍÓÎÎ‡È‰Â˚ ‚ÒÂı ‡ÍÚË‚Ì˚ı Ú‡ÈÎÓ‚
         
        }
        
    }
    //                                                                                           //
    //                                                                                           //

    private List<Tile> GetTileListByType(TileType type)
    {
        switch (type)
        {
            case TileType.Junk:
                return _junkTiles;

            case TileType.Iron:
                return _ironTiles;

            case TileType.Rubber:
                break;
            case TileType.Plastic:
                break;

        }
        return null;
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
    }
}

