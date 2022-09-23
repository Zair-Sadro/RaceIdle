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

    public void RemoveTiles(TileType type,Vector3 tilesPlace)
    {
        if (!_isGivingTiles)
            StartCoroutine(RemovingTile(type, tilesPlace));
    }

    private IEnumerator RemovingTile(TileType type, Vector3 tilesPlace)
    {

        _isGivingTiles = true;

        List<Tile> tiles =  GetTileListByType(type);
        if (tiles == null) yield break;

        for (int i = tiles.Count - 1; i >= 0; i--)
        {
            tiles[i].ThrowTo(tilesPlace, timeToRemoveTile);
            yield return new WaitForSeconds(timeToRemoveTile);

            _givenTiles.Add(tiles[i]);

            ClearTiles(i, type);


        }
        _isGivingTiles = false;
    }

    private void ClearTiles(int index,TileType type)
    {
        _colectedTiles[index].gameObject.SetActive(false);
        _colectedTiles[index].OnGround();
        _colectedTiles[index].transform.SetParent(tilesSpawnerParent);

                 _colectedTiles.Remove(_colectedTiles[index]);
        GetTileListByType(type).Remove(_colectedTiles[index]);

        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }
    private void ClearTiles(int index)
    {
        _colectedTiles[index].gameObject.SetActive(false);
        _colectedTiles[index].OnGround();
        _colectedTiles[index].transform.SetParent(tilesSpawnerParent);

        _colectedTiles.Remove(_colectedTiles[index]);
        GetTileListByType(_colectedTiles[index].Type).Remove(_colectedTiles[index]);

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
          
        }
        _colectedTiles.Clear();
        for (int i = 0; i < sortTiles.Count; i++)
        {
            sortTiles[i].gameObject.SetActive(true);
            AddTile(sortTiles[i]);
        }
    }

    public void RemoveTilesAtCount(int count)
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



    // ���������� ��� ����������� : ����� TILE ����� ����������� TRIGGERENTER PLAYER             //
    // � ����� ���������� TILESETTER ��� ������ ��� ����� TRYGETCOMPONENT                        //
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Tile tile) && _colectedTiles.Count < maxTiles)
        {
            AddTile(tile);

            if (MaxTilesCapacity())
                OnTilesMaxCapacity.Invoke(false); // �������� ���������� ���� �������� ������
         
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

    private void OnTriggerStay(Collider other)
    {
       
    }

    private void OnTriggerExit(Collider other)
    {
       
    }
   
}
