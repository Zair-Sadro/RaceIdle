using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

//CanBeInjected//
public class ResourceTilesSpawn : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private int maxTilesAmount;
    [SerializeField] private Transform _tilesParent;

    [SerializeField] private Material _junkMat, _plastMat, _ironMat, _rubberMat;


    [Inject] private TileSetter _tileSetter;
    private ObjectPooler<Tile> _tilesPool;


    private void OnEnable()
    {
        _tilesPool = new ObjectPooler<Tile>(_tilePrefab, _tilesParent);
        _tilesPool.CreatePool(maxTilesAmount);
    }

    public virtual List<Tile> GetRandomTiles(int min, int max)
    {
        List<Tile> newList = new List<Tile>();
        int randomAmount = Random.Range(min, max);

        for (int i = 0; i < randomAmount; i++)
            newList.Add(_tilesPool.GetFreeObject());

        return newList;
    }
    public Tile GetTile(TileType type)
    {
            var tile = _tilesPool.GetFreeObject();
            SetType(type);
            tile.InjectTileSetter(_tileSetter);

            if (_tileSetter.MaxTilesCapacity())
            {
                tile.SetColliderActive(false);
            }
            else
            {
                tile.SetColliderActive(true);
            }

            return tile;

        void SetType(TileType t)
        {
            tile.Type = t;
            switch (t)
            {
                case TileType.Junk:
                    tile.SetMaterial(_junkMat);
                    break;

                case TileType.Iron:
                    tile.SetMaterial(_ironMat);
                    break;

                case TileType.Rubber:
                    tile.SetMaterial(_rubberMat);
                    break;

                case TileType.Plastic:
                    tile.SetMaterial(_plastMat);
                    break;

            }
        }

    
    }
    public List<Tile> GetAllActiveTiles()
    {
       return _tilesPool.GetAllActiveObjects();
    }
}
