using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollector : MonoBehaviour
{



    protected Action<int,int> OnCountChange;

    [SerializeField] protected virtual int maxTileCount { get; private set; }
    [SerializeField] protected virtual int currentTilesCount { get; set; }
    [SerializeField] protected CounterView _counterView;


    [Zenject.Inject] protected TileSetter _playerTilesBag;
    [SerializeField] protected Transform tileStorage;
    [SerializeField] protected bool hideAfterCollect = true;

    //<<<<<<<<<<<<<<<<<<<<<<<< Required tiles fields >>>>>>>>>>>>>>>>>>>>>>>>>>>\\
    [SerializeField] protected virtual List<TileType> _requiredTypes { get; set; }
    [SerializeField] protected List<ProductRequierment> productRequierments;
    protected Dictionary<TileType, Stack<Tile>> tileListByType = new Dictionary<TileType, Stack<Tile>>();
    protected byte _requiredTypesCount;
    //<<<<<<<<<<<<<<<<<<<<<<<<                         >>>>>>>>>>>>>>>>>>>>>>>>>>>\\

    private void OnEnable()
    {
        if (_counterView == null) return;
        OnCountChange += _counterView.TextCountVisual;
    }
    private void OnDisable()
    {
        OnCountChange -= _counterView.TextCountVisual;
    }


    protected virtual void Collect()
    {if (_playerTilesBag._isGivingTiles) return;
        for (int i = 0; i < _requiredTypesCount; i++)
        {
            var req = _requiredTypes[i];
            _playerTilesBag.RemoveTiles(req, tileStorage.position, RecieveTile, hideAfterCollect);
        }
    }
    protected virtual void StopCollect()
    {
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




