using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    private WalletSystem _wallet => InstantcesContainer.Instance.WalletSystem;
    private TileSetter _tileSetter => InstantcesContainer.Instance.TileSetter;
    private StatsValuesInformator _statsValuesInformator => InstantcesContainer.Instance.StatsValuesInformator;

    [SerializeField] private TMPro.TMP_Text tx_money;
    [SerializeField] private TMPro.TMP_Text 
                                      tx_junk, 
                                      tx_iron,
                                      tx_plastic,
                                      tx_rubber;
    
    [SerializeField] private GameObject _iron,
                                        _plastic,
                                        _rubber;
    
    [SerializeField] private Vector3 _moneyTextPunchVector;

    private readonly Dictionary<TileType, TMPro.TMP_Text> textByTileType = new(4);
    private void Awake()
    {

        textByTileType.Add(TileType.Iron, tx_iron);
        textByTileType.Add(TileType.Junk, tx_junk);
        textByTileType.Add(TileType.Plastic, tx_plastic);
        textByTileType.Add(TileType.Rubber, tx_rubber);
       
    }

    private void Start()
    {
        SubscribeOnInit(_tileSetter.TilesListsByType);
        _wallet.OnTotalMoneyChange += MoneyView;
        
        SetActiveInventedTileType();

        void SetActiveInventedTileType()
        {
            _iron.SetActive(false);
            _rubber.SetActive(false);
            _plastic.SetActive(false);
            
            var listTypes = _statsValuesInformator.GetMaxGainedType();
            foreach (var type in listTypes)
            {
                switch (type)
                {
                    case TileType.Junk:
                        break;
                    case TileType.Iron:
                        _iron.SetActive(true);
                        break;
                    case TileType.Rubber:
                        _rubber.SetActive(true);
                        break;
                    case TileType.Plastic:
                        _plastic.SetActive(true);
                        break;
                }
            }
        }
    }


    private void MoneyView(float total)
    {
        tx_money.text = total.ToString("0.#");
        tx_money.transform.DORewind();
       tx_money.transform.DOPunchScale(_moneyTextPunchVector, 0.2f);
    }

    private void SubscribeOnInit(IReadOnlyDictionary<TileType,TileList> dict)
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

    private void ChangeTextTileCount(TileType type,int count)
    {
        CheckDisabledType(type);

        textByTileType[type].text = count.ToString();
        textByTileType[type].transform.DORewind();
        textByTileType[type].transform.DOPunchScale(_moneyTextPunchVector, 0.2f,5);
    }

    private void CheckDisabledType(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Junk:
                return;
            
            case TileType.Iron:
                
                if(!_iron.activeSelf)
                    _iron.SetActive(true);
                break;
            
            case TileType.Rubber:
                
                if(!_rubber.activeSelf)
                    _rubber.SetActive(true);
                break;
            
            case TileType.Plastic:
                
                if(!_plastic.activeSelf)
                    _plastic.SetActive(true);
                break;

        }
    }
}

