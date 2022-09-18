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

    private List<Tile> _tiles = new List<Tile>();
    private List<Tile> _givenTiles = new List<Tile>();

    public UnityEvent OnTilePickUp;
    public UnityEvent OnTileRemove;
    public event Action<int> OnTilesCountChanged;
    public event Action<bool> OnTilesMaxCapacity;
    public event Action<MachineTool> OnBenchZoneEnter;
    public event Action OnBenchZoneExit;

    public List<Tile> Tiles => _tiles;
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

        if (_tiles.Count % tilesRow == 0)
            tile.transform.localPosition = new Vector3(0, yOffset * _tiles.Count, 0);
        else
            tile.transform.localPosition = new Vector3(0, yOffset * (_tiles.Count - 1), zOffset);

        tile.transform.localRotation = Quaternion.Euler(0, 90, 0);
        tile.transform.localScale = tilesScale;
        _tiles.Add(tile);
        OnTilesCountChanged?.Invoke(_tiles.Count);
    }

    public void RemoveTiles(Action towerTileIncrease)
    {
        if (!_isGivingTiles)
            StartCoroutine(RemovingTile(towerTileIncrease));
    }

    private IEnumerator RemovingTile(Action towerTileIncrease)
    {
        _isGivingTiles = true;
        for (int i = _tiles.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(timeToRemoveTile);

            _givenTiles.Add(_tiles[i]);
            ClearTiles(i);
            OnTileRemove?.Invoke();
            towerTileIncrease();
        }
        _isGivingTiles = false;
    }

    private void ClearTiles(int index)
    {
        _tiles[index].gameObject.SetActive(false);
        _tiles[index].OnGround();
        _tiles[index].transform.SetParent(tilesSpawnerParent);
        _tiles.Remove(_tiles[index]);
        OnTilesCountChanged?.Invoke(_tiles.Count);
    }

    public void StopRemovingTiles()
    {
        _isGivingTiles = false;
        StopAllCoroutines();
        SortTiles();
    }

    private void SortTiles()
    {
        if (_tiles.Count <= 0)
            return;

        List<Tile> sortTiles = new List<Tile>();
        for (int i = _tiles.Count - 1; i >= 0; i--)
        {
            sortTiles.Add(_tiles[i]);
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
        if (_tiles.Count <= 0 || count > _tiles.Count)
            return;

        for (int i = count - 1; i >= 0; i--)
            ClearTiles(i);

        SortTiles();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Tile tile) && _tiles.Count < maxTiles)
        {
            AddTile(tile);
            OnTilePickUp?.Invoke();

            if (_tiles.Count == maxTiles)
                OnTilesMaxCapacity.Invoke(false);
         
        }
        
    }
    
    private void SetTilesColliderStatus(bool value)
    {
        var tiles = _resourceTilesSpawn.GetAllActiveTiles();
        foreach (var tile in tiles)
        {
            tile.SetColliderActive(value);
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out BaseMachineTool t))
        {
            _currentBench = t;
            OnBenchZoneEnter?.Invoke(t);
            _isInBenchZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out BaseMachineTool t))
        {
            _isInBenchZone = false;
            OnBenchZoneExit?.Invoke();
            StopRemovingTiles();
        }

        if (_currentBench != null)
            _currentBench = null;
    }
   
}
