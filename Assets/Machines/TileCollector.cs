using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollector : MonoBehaviour
{
    protected Action<TileType,int> OnCountChange;

    [SerializeField] protected virtual int maxTileCount { get; private set; }
    [SerializeField] protected virtual int currentTilesCount { get; set; }
    [SerializeField] protected CounterViewbase _counterView;


    protected TileSetter _playerTilesBag => InstantcesContainer.Instance.TileSetter;
    [SerializeField] protected Transform tileStorage;
    [SerializeField] protected bool hideAfterCollect = true;

    //<<<<<<<<<<<<<<<<<<<<<<<< Required tiles fields >>>>>>>>>>>>>>>>>>>>>>>>>>>\\
    [SerializeField] protected virtual List<TileType> _requiredTypes { get; set; }
    [SerializeField] protected List<ProductRequierment> productRequierments;

    protected Dictionary<TileType, Stack<Tile>> tileListByType = new();

    protected byte _requiredTypesCount;
    private bool _stopCollect;

    protected virtual void Collect()
    {
        if (_playerTilesBag._isGivingTiles) return;

        _stopCollect = false;
        StartCoroutine(CollectCor());

    }
    private IEnumerator CollectCor()
    {
        var reqtype = new List<TileType>(_requiredTypes);
        for (int i = 0; i < reqtype.Count; i++)
        {
            if (_stopCollect)
                yield break;

            var req = reqtype[i];
            var countneed = productRequierments[i].Amount - tileListByType[req].Count;

            if(countneed==0)
                continue;

            print($"await{req}");
            yield return StartCoroutine(_playerTilesBag.RemoveTilesWthCount
                (req, countneed, tileStorage.position, RecieveTile,true));

        }
    }
    protected virtual void StopCollect()
    {
        _stopCollect = true;
        _playerTilesBag.StopRemovingTiles();    
    }
    protected virtual void RecieveTile(Tile tile)
    {
        tileListByType[tile.Type].Push(tile);

    }

    protected void InitDictionary()
    {
        _requiredTypesCount = (byte)productRequierments.Count;

        _requiredTypes = new List<TileType>();

        for (int i = 0; i < _requiredTypesCount; i++)
        {
            _requiredTypes.Add(productRequierments[i].Type);

            tileListByType.Add(productRequierments[i].Type, new Stack<Tile>());
            
        }
    }
}




