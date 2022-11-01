﻿using System.Collections.Generic;
using UnityEngine;

public class ResourceView : MonoBehaviour
{
    [Zenject.Inject] private WalletSystem _wallet;
    [Zenject.Inject] private TileSetter _tileSetter;

    [SerializeField] private TMPro.TMP_Text tx_money;
    [SerializeField] private TMPro.TMP_Text 
                                      tx_junk, 
                                      tx_iron,
                                      tx_plastic,
                                      tx_rubber;

    private Dictionary<TileType, TMPro.TMP_Text> textByTileType = new(4);
    private void Awake()
    {

        textByTileType.Add(TileType.Iron, tx_iron);
        textByTileType.Add(TileType.Junk, tx_junk);
        textByTileType.Add(TileType.Plastic, tx_plastic);
        textByTileType.Add(TileType.Rubber, tx_rubber);

    }

    private void Start()
    {
        SubscribeOnInit(_tileSetter.tilesListsByType);
    }


    private void OnEnable()
    {
        _wallet.OnTotalMoneyChange += MoneyView;

    }
    private void OnDisable()
    {
        _wallet.OnTotalMoneyChange -= MoneyView;
    }

    private void MoneyView(float total)
    {
        tx_money.text = total.ToString(); 
    }

    private void SubscribeOnInit(Dictionary<TileType,TileList> dict)
    {
        dict[TileType.Junk].OnTileAdded += ChangeTextTileCount;
        dict[TileType.Iron].OnTileAdded += ChangeTextTileCount;
        dict[TileType.Plastic].OnTileAdded += ChangeTextTileCount;
        dict[TileType.Rubber].OnTileAdded += ChangeTextTileCount;

        dict[TileType.Junk].OnTileRemoved += ChangeTextTileCount;
        dict[TileType.Iron].OnTileRemoved += ChangeTextTileCount;
        dict[TileType.Plastic].OnTileRemoved += ChangeTextTileCount;
        dict[TileType.Rubber].OnTileRemoved += ChangeTextTileCount;
    }

    private void ChangeTextTileCount(TileListInfo tileListInfo)
    {
        var type = tileListInfo.ListType;
        var count = tileListInfo.Count;

        textByTileType[type].text = count.ToString();
    }
}

