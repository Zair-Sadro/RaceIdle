using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TileSetter : MonoBehaviour, ISaveLoad<TileSetterData>
{
    [SerializeField] private TileSetterData _data;
    [SerializeField] private Transform tilesSpawnerParent;

    [Header("Add to Unit Settings")]
    [SerializeField] private Transform setupPoint;
    [SerializeField] private int maxTiles;
    [SerializeField] private float powerTileJump = 2f;
    [Space]
    [SerializeField, Range(0.1f, 1f)] private float timeToRemoveTile;
    [SerializeField] private float delayToRemoveTile = 0.7f;

    private ResourceTilesSpawn _resourceTilesSpawn => InstantcesContainer.Instance.ResourceTilesSpawn;

    [HideInInspector] public bool _isGivingTiles;


    [SerializeField]
    private List<Tile> _colectedTiles = new List<Tile>();
    private TileList _junkTiles = new(TileType.Junk);
    private TileList _ironTiles = new(TileType.Iron);
    private TileList _plasticTiles = new(TileType.Plastic);
    private TileList _rubberTiles = new(TileType.Rubber);

    private Dictionary<TileType, TileList> tilesListsByType = new(4);
    public IReadOnlyDictionary<TileType, TileList> TilesListsByType => tilesListsByType;



    private bool _maxCapacity;
    public bool MaxCapacity => _maxCapacity;

    public event Action<int> OnTilesCountChanged;
    public event Action<bool> OnTilesMaxCapacity;

    private void Awake()
    {
        tilesListsByType[TileType.Junk] = _junkTiles;
        tilesListsByType[TileType.Iron] = _ironTiles;
        tilesListsByType[TileType.Plastic] = _plasticTiles;
        tilesListsByType[TileType.Rubber] = _rubberTiles;

    }

    private void Start()
    {
        GameEventSystem.TileSold += (type) => RemoveBySoldTile(type);
        GameEventSystem.TileBought += (type) => 
        { TryAddTile(type); 
            InstantcesContainer.Instance.AudioService.PlayAudo(AudioName.SHOP); 
        };
        GameEventSystem.SoldALl += RemoveAll;
    }
    private void RemoveAll()
    {
        for (int i = 0; i < _colectedTiles.Count; i++)
        {
            ClearTiles(_colectedTiles[i]);

        }
        _colectedTiles.Clear();
    }

    #region AddTile
    public bool TryAddTile(Tile tile)
    {
        if (_maxCapacity)
            return false;

        tile.OnTake();
        tile.JumpTween(setupPoint.position, powerTileJump, () =>
        {
            tile.transform.SetParent(setupPoint);
            tile.transform.localRotation = Quaternion.identity;
        });


        _colectedTiles.Add(tile);
        tilesListsByType[tile.Type].AddTile(tile);

        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
        CheckMaxTilesCapacity();
        return true;
    }
    /// <summary>
    /// Used only from tile shop after buy
    /// </summary>
    /// <param name="type"></param>
    public bool TryAddTile(TileType type)
    {
        var tile = _resourceTilesSpawn.GetTile(type);
        if (_maxCapacity) 
        {
            tile.ThrowTo(this.transform.position,StaticValues.tileThrowDelay);
            return false;
        }
        else 
        {
            tile.OnTake();
            tile.transform.SetParent(setupPoint);

            tile.transform.localRotation = Quaternion.identity;


            _colectedTiles.Add(tile);
            tilesListsByType[tile.Type].AddTile(tile);

            OnTilesCountChanged?.Invoke(_colectedTiles.Count);
            CheckMaxTilesCapacity();
            InstantcesContainer.Instance.AudioService.PlayAudo(AudioName.TILE);
            return true;
        }

       
    }
    private void CheckMaxTilesCapacity()
    {
        _maxCapacity = _colectedTiles.Count >= maxTiles;
        if(_maxCapacity)
            OnTilesMaxCapacity.Invoke(false);

    }

    #endregion

    #region RemoveTile

    /// <summary>
    /// Remove with count
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    /// <param name="tilesPlace"></param>
    /// <param name="interatorCall"></param>
    /// <param name="needClear"></param> 
    public IEnumerator RemoveTilesWthCount(TileType type, int count, Vector3 tilesPlace, Action<Tile> interatorCall, bool needClear = false)
    {

        if (!_isGivingTiles && tilesListsByType[type].Count > 0)
        {
            var finalCount = CalculateAvailableCount(count, tilesListsByType[type].Count);
            yield return RemovingTileCount(type, finalCount, tilesPlace, interatorCall, needClear);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        int CalculateAvailableCount(int countneed, int countnow)
        {
            if (countnow < countneed) return countnow;
            else return countneed;
        }

    }
    private IEnumerator RemovingTileCount(TileType type, int count, Vector3 tilesPlace, Action<Tile> interatorCall, bool needClear)
    {
        _isGivingTiles = true;

        TileList tiles = tilesListsByType[type];

        yield return new WaitForSeconds(delayToRemoveTile);

        for (int i = count - 1; i >= 0; i--)
        {
            print($"removed{type}:{i}");
            tiles[i].ThrowTo(tilesPlace, timeToRemoveTile);
            interatorCall?.Invoke(tiles[i]);
            InstantcesContainer.Instance.AudioService.PlayAudo(AudioName.TILE);
            yield return WaitAndClearTile(needClear, tiles[i]);

            if (_isGivingTiles == false)
                yield break;
        }
        _isGivingTiles = false;
    }

    /// <summary>
    /// Remove anycount
    /// </summary>
    /// <param name="type"></param>
    /// <param name="tilesPlace"></param>
    /// <param name="interatorCall"></param>
    /// <param name="needClear"></param>
    public void RemoveTiles(TileType type, Vector3 tilesPlace, Action<Tile> interatorCall, bool needClear = false)
    {
        if (!_isGivingTiles && tilesListsByType[type].Count > 0)
        {
            _isGivingTiles = true;
            StartCoroutine(RemovingTile(type, tilesPlace, interatorCall, needClear));
        }
           
    }
    private IEnumerator RemovingTile(TileType type, Vector3 tilesPlace, Action<Tile> interatorCall, bool needClear)
    {

        TileList neededTilesList = tilesListsByType[type];

        yield return new WaitForSeconds(delayToRemoveTile);
        for (int i = neededTilesList.Count - 1; i >= 0; i--)
        {
            neededTilesList[i].ThrowTo(tilesPlace, timeToRemoveTile);
            interatorCall?.Invoke(neededTilesList[i]);
            InstantcesContainer.Instance.AudioService.PlayAudo(AudioName.TILE);
            yield return WaitAndClearTile(needClear, neededTilesList[i]);

            if (_isGivingTiles == false)
                yield break;
        }
        _isGivingTiles = false;
    }

    private void RemoveBySoldTile(TileType type)
    {
        StartCoroutine(RemoveOneTile(type));
    }
    private IEnumerator RemoveOneTile(TileType type)
    {
        TileList neededTilesList = tilesListsByType[type];
        var last = neededTilesList.Count - 1;
        InstantcesContainer.Instance.AudioService.PlayAudo(AudioName.SHOP);
        yield return WaitAndClearTile(true, neededTilesList[last]);


    }

    #endregion
    IEnumerator WaitAndClearTile(bool needclear, Tile tile)
    {

        if (needclear)
        {
            if(tile.gameObject.activeSelf)
            ClearTiles(tile, timeToRemoveTile);
        }
        else
            RemoveFromList(tile);

        if (_maxCapacity) 
        {
            OnTilesMaxCapacity.Invoke(false);
            _maxCapacity = false;
        }
         

        yield return new WaitForSeconds(0.1f);
    }

    private void RemoveFromList(Tile tile)
    {
        _colectedTiles.Remove(tile);
        tilesListsByType[tile.Type].RemoveTile(tile);
       

        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }

    private void ClearTiles(Tile tile, float timer = 0)
    {
        _colectedTiles.Remove(tile);
        tile.Dissapear(timer);
        tile.transform.SetParent(tilesSpawnerParent);

        tilesListsByType[tile.Type].RemoveTile(tile);
      
        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }
    public void StopRemovingTiles()
    {
        _isGivingTiles = false;

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


