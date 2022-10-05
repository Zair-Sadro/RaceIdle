using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollector : MonoBehaviour
{


    [SerializeField] protected Animator counterAnimator;

    [SerializeField] protected TMPro.TMP_Text counter;

    protected Action<int> OnCountChange;

    [SerializeField] protected virtual int maxTileCount { get; private set; }
    [SerializeField] protected virtual int currentTilesCount { get; set; }



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
        if (counter == null) return;
        OnCountChange += TextCountVisual;
    }
    private void OnDisable()
    {
        OnCountChange -= TextCountVisual;
    }
    public virtual void TextCountVisual(int maxValue)
    {
        counter.text = $"{currentTilesCount}/{maxValue}";
        counterAnimator.SetTrigger("Plus");
    }

    protected virtual void Collect()
    {
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

