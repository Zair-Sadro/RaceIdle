using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;
using Cysharp.Threading.Tasks;

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
        _timeToRemove = timeToRemoveTile * 1.1f;
        tilesListsByType[TileType.Junk] = _junkTiles;
        tilesListsByType[TileType.Iron] = _ironTiles;
        tilesListsByType[TileType.Plastic] = _plasticTiles;
        tilesListsByType[TileType.Rubber] = _rubberTiles;
    }

    private void Start()
    {
        GameEventSystem.TileSold +=((type)=> RemoveTiles(type,Vector3.zero,null,true));
        GameEventSystem.TileBought += AddTile;
        GameEventSystem.SoldALl += RemoveAll;
    }

    #region AddTile
    public void AddTile(Tile tile)
    {

        tile.OnTake();
        tile.JumpTween(setupPoint.position, powerTileJump, () =>
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

        var tile = _resourceTilesSpawn.GetTile(type);
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
    public UniTask RemoveTiles(TileType type,int count, Vector3 tilesPlace, Action<Tile> interatorCall, System.Threading.CancellationTokenSource cancellationToke, bool needClear = false)
    {

        if (!_isGivingTiles && tilesListsByType[type].Count > 0)
        {
            var c = CalculateAvailableCount(count, tilesListsByType[type].Count);
            return RemovingTile(type, c, tilesPlace, interatorCall, needClear).ToUniTask(cancellationToken: cancellationToke.Token);
        }
        else
        {
            return UniTask.DelayFrame(100, PlayerLoopTiming.Update,cancellationToke.Token);
        }

        int CalculateAvailableCount(int countneed,int countnow)
        {
            if (countnow < countneed) return countnow;
            else return countneed;
        }
           
    }
    private IEnumerator RemovingTile(TileType type, int count,Vector3 tilesPlace, Action<Tile> interatorCall, bool needClear)
    {
        _isGivingTiles = true;

        TileList tiles = tilesListsByType[type];

        yield return new WaitForSeconds(delayToRemoveTile);
        
        for (int i = count-1; i >= 0; i--)
        {
            print($"removed{type}:{i}");
            tiles[i].ThrowTo(tilesPlace, timeToRemoveTile);
            interatorCall?.Invoke(tiles[i]);

            yield return WaitAndClearTile(timeToRemoveTile, needClear, tiles[i]);

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
    public void RemoveTiles(TileType type,Vector3 tilesPlace,Action<Tile> interatorCall, bool needClear = false)
    {
        if (!_isGivingTiles && tilesListsByType[type].Count>0)
            StartCoroutine(RemovingTile(type, tilesPlace, interatorCall, needClear));
    }
    private IEnumerator RemovingTile(TileType type, Vector3 tilesPlace,Action<Tile> interatorCall,bool needClear)
    {
        

        _isGivingTiles = true;

        TileList tiles = tilesListsByType[type];

        yield return new WaitForSeconds(delayToRemoveTile);

        for (int i = tiles.Count-1; i >= 0; i--)
        {
            tiles[i].ThrowTo(tilesPlace, timeToRemoveTile);
            interatorCall?.Invoke(tiles[i]);

            yield return WaitAndClearTile(timeToRemoveTile, needClear, tiles[i]);
      
            if (_isGivingTiles == false)
                yield break;
        }
        _isGivingTiles = false;
    }
    private void RemoveAll()
    {
        for (int i = 0; i < _colectedTiles.Count; i++)
        {
            ClearTiles(_colectedTiles[i]);

        }
        _colectedTiles.Clear();
    }
    #endregion

    IEnumerator WaitAndClearTile(float delay,bool needclear,Tile tile)
    {
        yield return new WaitForSeconds(delay);

        if (needclear)
            ClearTiles(tile);
        else
            RemoveFromList(tile);
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
    public void StopRemovingTiles()
    {
        _isGivingTiles = false;

    }


    public bool MaxTilesCapacity()
    {
        maxCapacity = _colectedTiles.Count == maxTiles;
        return maxCapacity;

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


