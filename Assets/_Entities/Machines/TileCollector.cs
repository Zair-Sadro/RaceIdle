using System;
using System.Collections.Generic;
using UnityEngine;

public class TileCollector : MonoBehaviour
{

    [SerializeField] protected CounterViewbase _counterView;
    [SerializeField] protected Transform tileStorage;
    [SerializeField] protected List<ProductRequierment> productRequierments;

    protected TileSetter _playerTilesBag => InstantcesContainer.Instance.TileSetter;
    protected List<TileType> _requiredTypes { get; set; }
    protected Dictionary<TileType, Stack<Tile>> tileListByType = new();
    protected Action<TileType, int> OnCountChange;
    protected virtual int maxTileCount { get; private set; }
    protected int currentTilesCount { get; set; }
    protected int goldCount;

    protected byte _requiredTypesCount;
    protected bool _stopCollect;

    protected void StopCollect()
    {
        _stopCollect = true;
        _playerTilesBag.StopRemovingTiles();
    }
    protected virtual void RecieveTile(Tile tile)
    {
        tileListByType[tile.Type].Push(tile);

    }
    protected virtual void RecieveGold(int goldAmount)
    {
        this.goldCount += goldAmount;
    }

    protected void InitDictionary()
    {
        _requiredTypesCount = (byte)productRequierments.Count;

        _requiredTypes = new List<TileType>();

        for (int i = 0; i < _requiredTypesCount; i++)
        {
            var type = productRequierments[i].Type;
            _requiredTypes.Add(type);

            if (type == TileType.Gold)
            {
                goldCount = 0;
            }
            else
            {
                tileListByType.Add(type, new Stack<Tile>());
            }


        }
    }
    
}




