
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    private TileSetter _tileSetter => InstantcesContainer.Instance.TileSetter;
    private StatsValuesInformator _statsValuesInformator => InstantcesContainer.Instance.StatsValuesInformator;

    [Header("Panels")]
    [SerializeField] private UIPanel _buyPanel;
    [SerializeField] private UIPanel _sellPanel;

    [Tooltip("MultiplyValue x MachineIncome"),
     Header("Multiply to price")]
    [SerializeField] private float _sellMulti = 0.5f;
    [SerializeField] private float _buyMulti = 2.5f;

    #region Calculation & field to get calcilation info

    private Dictionary<TileType, TileEcoInfo> _priceByTypeDict = new() 
    { 
     { TileType.Junk, new TileEcoInfo(TileType.Junk) },
     { TileType.Iron, new TileEcoInfo(TileType.Iron) },
     { TileType.Rubber, new TileEcoInfo(TileType.Rubber) },
     { TileType.Plastic, new TileEcoInfo(TileType.Plastic) }
    };
    public IReadOnlyDictionary<TileType, TileEcoInfo> PriceInfo => _priceByTypeDict;

    private void CalculateSellPrice()
    {
        var dictionary = _tileSetter.TilesListsByType;

        foreach (var element in dictionary)
            if (dictionary.TryGetValue(element.Key, out var t))
            {
                if (t.Count > 0)
                {
                    TileEcoInfo tileEcoInfo = new TileEcoInfo(t.Type, t.Count, CalculateTilePrice(t.Type));
                    _priceByTypeDict[t.Type] = tileEcoInfo;

                }
                else
                {
                    _priceByTypeDict[t.Type] = new TileEcoInfo(t.Type);
                }
            }

        float CalculateTilePrice(TileType type)
        {
            float price=0;
            switch (type)
            {
                case TileType.Junk:
                    price = 1;
                    break;

                case TileType.Iron:
                    price = 2;
                    break;

                case TileType.Rubber:
                    price = 3;
                    break;

                case TileType.Plastic:
                    price = 4.5f;
                    break;

                default:

                    break;
            }
            //var price = _statsValuesInformator.GetMachineIncome(type) * _sellMulti + 1f;
            return price;
        }


    }
    private void CalculateBuyPrice()
    {

        var listTypes = _statsValuesInformator.GetMaxGainedType();     

        foreach (var type in listTypes)
        {
            TileEcoInfo tileEcoInfo = new TileEcoInfo(type, 0, CalculateTilePrice(type));
            _priceByTypeDict[type] = tileEcoInfo;
        }

        float CalculateTilePrice(TileType type)
        {
            float price = 2;
            switch (type)
            {
                case TileType.Junk:
                    price = 2;
                    break;

                case TileType.Iron:
                    price = 4;
                    break;

                case TileType.Rubber:
                    price = 6;
                    break;

                case TileType.Plastic:
                    price = 9;
                    break;

                default:
                    break;
            }
            //var price = _statsValuesInformator.GetMachineIncome(type) * _buyMulti;
            return price;
        }

    }
    #endregion

    private void OnEnable()
    {

        ShowPanelAfterDetectPlayer();
    }

    #region Detect player And Show Panel
    [SerializeField] PlayerDetector _playerDetectOnSell;
    [SerializeField] PlayerDetector _playerDetectOnBuy;
    private void ShowSellOffer()
    {
        CalculateSellPrice();
        _sellPanel.Show();
    }
    private void ShowBuyOffer()
    {
        CalculateBuyPrice();
        _buyPanel.Show();
    }

    private void ShowPanelAfterDetectPlayer()
    {
        _playerDetectOnSell.OnPlayerEnter += ShowSellOffer;
        _playerDetectOnBuy.OnPlayerEnter += ShowBuyOffer;
    }


    #endregion



}
public struct TileEcoInfo
{
    public int count;
    public float price;
    public TileType type;
    public TileEcoInfo(TileType type, int count, float price)
    {
        this.type = type;
        this.count = count;
        this.price = price;
    }
    public TileEcoInfo(TileType type)
    {
        this.type = type;
        this.count = 0;
        this.price = 0;
    }

}







