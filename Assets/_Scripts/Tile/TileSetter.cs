using AutoLayout3D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TileSetter : MonoBehaviour, ISaveLoad<TileSetterData>
{
    [SerializeField] private TileSetterData _data;
    [SerializeField] private Transform tilesSpawnerParent;
    [SerializeField] private GridLayoutGroup3D _layoutGroup;

    [Header("Add to Unit Settings")]
    [SerializeField] private Transform setupPoint;
    [SerializeField] private int maxTiles;
    [SerializeField] private float powerTileJump = 2f;
    [Space]
    [SerializeField, Range(0.1f, 1f)] private float timeToRemoveTile;
    [SerializeField] private float delayToRemoveTile = 0.7f;

    private ResourceTilesSpawn _resourceTilesSpawn => InstantcesContainer.Instance.ResourceTilesSpawn;
    private WalletSystem _wallet=> InstantcesContainer.Instance.WalletSystem;

    [HideInInspector] public bool _isGivingTiles;


    [SerializeField]
    private List<Tile> _colectedTiles = new List<Tile>();

    private TileList _junkTiles = new(TileType.Junk);
    private TileList _ironTiles = new(TileType.Iron);
    private TileList _plasticTiles = new(TileType.Plastic);
    private TileList _rubberTiles = new(TileType.Rubber);

    private Dictionary<TileType, TileList> tilesListsByType = new(4);
    public IReadOnlyDictionary<TileType, TileList> TilesListsByType => tilesListsByType;



    private bool _isMaxCapacity;

    public float CurrentGold => _wallet.TotalMoney;
    public bool IsMaxCapacity => _isMaxCapacity;
    public int MaxCapacity => maxTiles;

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
        {
            TryAddTile(type);
            InstantcesContainer.Instance.AudioService.PlayAudo(AudioName.SHOP);
        };
        GameEventSystem.SoldALl += RemoveAll;
        if (_data != null) 
        {
            AddTileByInit(_data._junkTiles, TileType.Junk);
            AddTileByInit(_data._ironTiles, TileType.Iron);
            AddTileByInit(_data._rubberTiles, TileType.Rubber);
            AddTileByInit(_data._plasticTiles, TileType.Plastic);
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

    #region AddTile
    public bool TryAddTile(Tile tile)
    {
        if (_isMaxCapacity || _isGivingTiles)
            return false;

        _layoutGroup.UpdateLayout();

        tile.OnTake();
        tile.JumpTween(setupPoint.position, powerTileJump, () =>
        {
            tile.transform.SetParent(setupPoint);
            tile.transform.localRotation = Quaternion.identity;
            _layoutGroup.UpdateLayout();
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
        if (_isMaxCapacity)
        {
            tile.ThrowTo(this.transform.position, StaticValues.tileThrowDelay,true);
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
            _layoutGroup.UpdateLayout();
            return true;
        }


    }
    private void CheckMaxTilesCapacity()
    {
        _isMaxCapacity = _colectedTiles.Count >= maxTiles;
        if (_isMaxCapacity)
            OnTilesMaxCapacity?.Invoke(false);

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
            yield return new WaitForSeconds(0.1f);
        }

        int CalculateAvailableCount(int countneed, int countnow)
        {
            if (countnow < countneed) return countnow;
            else return countneed;
        }

    }
    public IEnumerator RemoveGoldWthCount(int count, Vector3 tilesPlace, Action<int> interatorCall)
    {

        if (CurrentGold > 0)
        {
            var finalCount = CalculateAvailableCount(count, (int)CurrentGold);
            _wallet.TrySpendMoney(finalCount);
            interatorCall.Invoke(finalCount);
        }
        else
        {
            yield return new WaitForSeconds(0.15f);
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
            var tile = neededTilesList[i];
            tile.ThrowTo(tilesPlace, timeToRemoveTile);
            interatorCall?.Invoke(tile);
            // InstantcesContainer.Instance.AudioService.PlayAudo(AudioName.TILE);
            yield return WaitAndClearTile(needClear, tile);

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
        yield return StartCoroutine(WaitAndClearTile(true, neededTilesList[last]));


    }

    #endregion
    IEnumerator WaitAndClearTile(bool needclear, Tile tile)
    {

        if (needclear)
        {
            if (tile.gameObject.activeSelf)
                ClearTiles(tile, timeToRemoveTile);
        }
        RemoveFromList(tile);

        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
        if (_isMaxCapacity)
        {
            OnTilesMaxCapacity?.Invoke(false);
            _isMaxCapacity = false;
        }


        yield return new WaitForSeconds(0.1f);
        _layoutGroup.UpdateLayout();
    }

    private void RemoveFromList(Tile tile)
    {
        _colectedTiles.Remove(tile);
        tilesListsByType[tile.Type].RemoveTile(tile);

    }

    private void ClearTiles(Tile tile, float timer = 0)
    {
        tile.Dissapear(timer);
        tile.transform.SetParent(tilesSpawnerParent);


    }
    public void StopRemovingTiles()
    {
        _isGivingTiles = false;

    }

    public void SetCapacity(int value) 
    {
        maxTiles = value;
        _isMaxCapacity = false;
    }

    public void SetBonus(int value,float duration)
    {
        StartCoroutine(MaxTilesTempBonus(value, duration));

    }
    IEnumerator MaxTilesTempBonus(int value,float duration) 
    {
        var t = maxTiles;
        maxTiles = value;

        yield return new WaitForSeconds(duration);
        maxTiles = t;
    }
    public TileSetterData GetData()
    {
        _data._ironTiles = _ironTiles.Count;
        _data._junkTiles = _junkTiles.Count;
        _data._rubberTiles = _rubberTiles.Count;
        _data._plasticTiles = _plasticTiles.Count;

        return _data;
    }
    private void AddTileByInit(int count, TileType type)
    {
        for (int i = 0; i < count; i++)
        {
            var tile = _resourceTilesSpawn.GetTile(type);

            tile.OnTake();
            tile.transform.SetParent(setupPoint);

            tile.transform.localRotation = Quaternion.identity;


            _colectedTiles.Add(tile);
            tilesListsByType[tile.Type].AddTile(tile);
        }

        _layoutGroup.UpdateLayout();
    }
    public void Initialize(TileSetterData data)
    {
        _data = data;


    }
}

[Serializable]
public class TileSetterData
{

    public int _junkTiles;
    public int _ironTiles;
    public int _plasticTiles;
    public int _rubberTiles;

    public TileSetterData() 
    {
     _junkTiles=0;
     _ironTiles = 0;
     _plasticTiles = 0;
     _rubberTiles = 0;
    }

}


