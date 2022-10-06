using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
    [SerializeField] private int maxTiles;
    [Space]
    [SerializeField,Range(0.1f,1f)] private float timeToRemoveTile;

    [Inject] private ResourceTilesSpawn _resourceTilesSpawn;

    [HideInInspector] public bool _isGivingTiles;


    [SerializeField]
    private List<Tile> _colectedTiles = new List<Tile>();
    private List<Tile> _junkTiles = new List<Tile>();
    private List<Tile> _ironTiles = new List<Tile>();


    private bool maxCapacity;

    public event Action<int> OnTilesCountChanged;
    public event Action<bool> OnTilesMaxCapacity;


    public List<Tile> Tiles => _colectedTiles;



    private void Start()
    {
        OnTilesMaxCapacity += SetTilesColliderStatus;
    }


    public void AddTile(Tile tile)
    {
        if (MaxTilesCapacity())
        {
            OnTilesMaxCapacity.Invoke(false); // Отлючаем коллайдеры всех активных тайлов
            return;
        }
        tile.OnTake();       
        tile.transform.SetParent(setupPoint);

        tile.transform.localRotation = Quaternion.Euler(0, 0, 0);

        _colectedTiles.Add(tile);
        GetTileListByType(tile.Type).Add(tile);

        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }

    public void RemoveTiles(TileType type,Vector3 tilesPlace,Action<Tile> interatorCall, bool needClear = false)
    {
        if (!_isGivingTiles)
            StartCoroutine(RemovingTile(type, tilesPlace, interatorCall, needClear));
    }

    private IEnumerator RemovingTile(TileType type, Vector3 tilesPlace,Action<Tile> interatorCall,bool needClear)
    {

        _isGivingTiles = true;

        List<Tile> tiles =  GetTileListByType(type);
        if (tiles == null) yield break;

        for (int i = tiles.Count-1; i >= 0; i--)
        {
            tiles[i].ThrowTo(tilesPlace, timeToRemoveTile);
            interatorCall.Invoke(tiles[i]);
            yield return new WaitForSeconds(timeToRemoveTile);

            if (needClear)
                ClearTiles(tiles[i], type);
            else
                RemoveFromList(tiles[i], type);

            if (_isGivingTiles == false)
                yield break;
        }
        _isGivingTiles = false;
    }

    private void RemoveFromList(Tile tile, TileType type)
    {
        GetTileListByType(type).Remove(tile);
        _colectedTiles.Remove(tile);


        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }

    private void ClearTiles(Tile tile,TileType type)
    {
        tile.gameObject.SetActive(false);
        tile.OnGround();
        tile.transform.SetParent(tilesSpawnerParent);

        GetTileListByType(type).Remove(tile);
        _colectedTiles.Remove(tile);


        OnTilesCountChanged?.Invoke(_colectedTiles.Count);
    }

    public void StopRemovingTiles()
    {

        _isGivingTiles = false;

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

