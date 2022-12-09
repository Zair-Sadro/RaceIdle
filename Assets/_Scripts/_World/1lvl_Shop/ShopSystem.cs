
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem: MonoBehaviour
{
    [Zenject.Inject] private TileSetter _tileSetter;
    [Zenject.Inject] private StatsValuesInformator _statsValuesInformator;

    [Header("Panels")]
    [SerializeField] private UIPanel _buyPanel;
    [SerializeField] private UIPanel _sellPanel;

    [Tooltip("MultiplyValue x MachineIncome"),
     Header("Multiply to price")]
    [SerializeField] private float _sellMulti=0.5f;
    [SerializeField] private float _buyMulti=0.5f;

    #region Calculation & field to get calcilation info

    private Dictionary<TileType, TileEcoInfo> _priceByTypeDict = new();
    public IReadOnlyDictionary<TileType, TileEcoInfo> PriceInfo => _priceByTypeDict;
    
    private void CalculateSellPrice()
    {
        var dictionary = _tileSetter.TilesListsByType;

        _priceByTypeDict.Clear();////////////////////REFACTOR();////////////////////REFACTOR

        foreach (var element in dictionary)
            if (dictionary.TryGetValue(element.Key, out var t))
            {
                if (t.Count > 0)
                {
                    TileEcoInfo tileEcoInfo = new TileEcoInfo(t.Type, t.Count, CalculateTilePrice(t.Type));
                    _priceByTypeDict.Add(t.Type, tileEcoInfo);

                }
            }

        float CalculateTilePrice(TileType type)
        {
            var price = _statsValuesInformator.GetMachineIncome(type) * _sellMulti+1f;
            return price;
        }


    }
    private void CalculateBuyPrice()
    {
        var listTypes = _statsValuesInformator.GetMaxGainedType();

        _priceByTypeDict.Clear(); ////////////////////REFACTOR();////////////////////REFACTOR
        foreach (var type in listTypes) 
        {
            TileEcoInfo tileEcoInfo = new TileEcoInfo(type, 0, CalculateTilePrice(type));
            _priceByTypeDict.Add(type, tileEcoInfo);
        }

        float CalculateTilePrice(TileType type)
        {
            var price = _statsValuesInformator.GetMachineIncome(type) * _buyMulti;
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
            
}







