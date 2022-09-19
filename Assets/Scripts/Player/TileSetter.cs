using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Events;
using Zenject;

public class TileSetter : MonoBehaviour
{
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
    [SerializeField] private float timeToRemoveTile;

    [Inject] private ResourceTilesSpawn _resourceTilesSpawn;

    private float _currentRemovingTime;
    private bool _isInBenchZone;
    private bool _isGivingTiles;

    private BaseMachineTool _currentBench;

    private List<Tile> _colectedTiles = new List<Tile>();
    private List<Tile> _givenTiles = new List<Tile>();

    public UnityEvent OnTilePickUp;
    public UnityEvent OnTileRemove;
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

        if (_colectedTiles.Count % tilesRow == 0)
            tile.transform.localPosition = new Vector3(0, yOffset * _colectedTiles.Count, 0);
        else
            tile.transform.localPosition = new Vector3(0, yOffset * (_colectedTiles.Count - 1), zOffset);

        tile.transform.localRotation = Quaternion.Euler(0, 90, 0);
        tile.transform.localScale = tilesScale;
        _colectedTiles.Add(tile);
        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }

    public void RemoveTiles(Action towerTileIncrease,Vector3 tilesPlace)
    {
        if (!_isGivingTiles)
            StartCoroutine(RemovingTile(towerTileIncrease, tilesPlace));
    }

    private IEnumerator RemovingTile(Action towerTileIncrease, Vector3 tilesPlace)
    {
        _isGivingTiles = true;
        for (int i = _colectedTiles.Count - 1; i >= 0; i--)
        {
            _colectedTiles[i].ThrowTo(tilesPlace, timeToRemoveTile);
            yield return new WaitForSeconds(timeToRemoveTile);

            _givenTiles.Add(_colectedTiles[i]);
            ClearTiles(i);
            OnTileRemove?.Invoke();
            towerTileIncrease();
        }
        _isGivingTiles = false;
    }

    private void ClearTiles(int index)
    {
        _colectedTiles[index].gameObject.SetActive(false);
        _colectedTiles[index].OnGround();
        _colectedTiles[index].transform.SetParent(tilesSpawnerParent);
        _colectedTiles.Remove(_colectedTiles[index]);
        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }

    public void StopRemovingTiles()
    {
        _isGivingTiles = false;
        StopAllCoroutines();
        SortTiles();
    }

    private void SortTiles()
    {
        if (_colectedTiles.Count <= 0)
            return;

        List<Tile> sortTiles = new List<Tile>();
        for (int i = _colectedTiles.Count - 1; i >= 0; i--)
        {
            sortTiles.Add(_colectedTiles[i]);
            ClearTiles(i);
        }

        for (int i = 0; i < sortTiles.Count; i++)
        {
            sortTiles[i].gameObject.SetActive(true);
            AddTile(sortTiles[i]);
        }
    }

    public  void RemoveTilesAtCount(int count)
    {
        if (_colectedTiles.Count <= 0 || count > _colectedTiles.Count)
            return;

        for (int i = count - 1; i >= 0; i--)
            ClearTiles(i);

        SortTiles();
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
            OnTilePickUp?.Invoke();

            if (MaxTilesCapacity())
                OnTilesMaxCapacity.Invoke(false); // ŒÚÎ˛˜‡ÂÏ ÍÓÎÎ‡È‰Â˚ ‚ÒÂı ‡ÍÚË‚Ì˚ı Ú‡ÈÎÓ‚
         
        }
        
    }
    //                                                                                           //
    //                                                                                           //



    private void OnTriggerStay(Collider other)
    {
       
    }

    private void OnTriggerExit(Collider other)
    {
       
    }
   
}
