using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem: MonoBehaviour
{
    [Zenject.Inject] private TileSetter _tileSetter;
    [Zenject.Inject] private StatsValuesInformator _statsValuesInformator;

    [Header("UI & Buttons")]
    [SerializeField] private UIPanel _buyPanel;
    [SerializeField] private UIPanel _sellPanel;
    [SerializeField] private Button _sellAllButton;
    [SerializeField] private Button _SellUIButton;
    [SerializeField] private Button _BuyUIButton;

    #region Calculation & field to get calcilation info

    private Dictionary<TileType, TileEcoInfo> _priceByTypeDict = new();
    public IReadOnlyDictionary<TileType, TileEcoInfo> PriceInfo => _priceByTypeDict;

    private void CalculateSellPrice()
    {
        var dictionary = _tileSetter.TilesListsByType;

        _priceByTypeDict.Clear();

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
            var price = _statsValuesInformator.GetMachineIncome(type) * 0.3f;
            return price;
        }


    }
    private void CalculateBuyPrice()
    {
        var dictionary = _tileSetter.TilesListsByType;

        _priceByTypeDict.Clear();

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
            var price = _statsValuesInformator.GetMachineIncome(type) * 0.3f;
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

public class ShopUI : UIPanel
{
    [SerializeField] private ShopSystem _sellSystem;
    [SerializeField] private List<GameObject> _slots;

    [SerializeField] private Button _sellAllButt;

    private List<IRegisterSlot> _registerSlots;
  
    protected override void Start()
    {
        base.Start();
        GetIRegister();
        PanelInit(GetPanelAnimation());
    }
    private void GetIRegister()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _registerSlots.Add(_slots[i].GetComponent<IRegisterSlot>());
        }
    }

    public override void Show()
    {
        RegisterSlots();
        base.Show();
    }
    public override void Hide()
    {
        base.Hide();
        foreach (var slot in _slots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    private void RegisterSlots()
    {
        var dict =_sellSystem.PriceInfo;
        var i = 0;
        foreach (var item in dict)
        {    i++;
            _registerSlots[i].RegisterSlot(item.Value.type, item.Value.price, item.Value.count);
        }

       
    }
    #region PanelAnimations

    [SerializeField, Tooltip("Скорость появления панели")]
    private float _showSpeed = 0.5f;
    private Tween GetPanelAnimation()
    {
        return transform
       .DOMoveY(0, _showSpeed)
       .SetEase(Ease.InOutFlash).Pause();
    }
    #endregion
}

public class SellSlot : MonoBehaviour, IRegisterSlot
{
     [SerializeField] private TMP_Text _priceText;
     [SerializeField] private TMP_Text _countTileText;

     [SerializeField] private Button _sellOneButt;

     [Zenject.Inject] private WalletSystem _wallet;

     private TileType _type;
     private float _price;
     private float _count;

    // same as constructor or Instantiate() slot with needed info 
    public void RegisterSlot(TileType type,float price,int count)
    {
        _type = type;
        _price = price;
        _count = count;

        _countTileText.text = count.ToString();
        _priceText.text = price.ToString();

        gameObject.SetActive(true);
    }

    #region Events
    private void OnEnable()
    {
        _sellOneButt.onClick.AddListener(() => OnSell());   
    }

    private void OnDisable()
    {
        _sellOneButt.onClick.RemoveAllListeners();
    }


    private void OnSell()
    {
        GameEventSystem.TileSold.Invoke(_type);
        --_count;
        _wallet.Income(_price);

        if (_count <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion


}
public class BuySLot : MonoBehaviour, IRegisterSlot
{

    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _countTileText;

    [SerializeField] private Button _buyOneButt;

    [Zenject.Inject] private WalletSystem _wallet;
    [Zenject.Inject] private TileSetter _tileSetter;

    private TileType _type;
    private float _price;
    private float _count;

    // same as constructor or Instantiate() slot with needed info 
    public void RegisterSlot(TileType type, float price, int count)
    {
        _type = type;
        _price = price;
        _count = count;

        _countTileText.text = count.ToString();
        _priceText.text = price.ToString();

        Reprice(_wallet.TotalMoney);
        gameObject.SetActive(true);
    }

    #region Events
    private void OnEnable()
    {
        _wallet.OnTotalMoneyChange += Reprice;
        _buyOneButt.onClick.AddListener(() => OnBuy());
        _tileSetter.OnTilesMaxCapacity += OnMaxCount;
    }

    private void OnDisable()
    {
        _buyOneButt.onClick.RemoveAllListeners();
        _wallet.OnTotalMoneyChange -= Reprice;
        _tileSetter.OnTilesMaxCapacity -= OnMaxCount;
    }


    private void OnBuy()
    {
        GameEventSystem.TileBought.Invoke(_type);

        --_count;

        if (_count <= 0)
        {
            gameObject.SetActive(false);
        }
    }


    private void Reprice(float walletMoney)
    {
        _buyOneButt.interactable = (_price <= walletMoney);
    }
    private void OnMaxCount(bool b)
    {
        _buyOneButt.interactable = false;
    }
    #endregion


}

public interface IRegisterSlot
{   
    public void RegisterSlot(TileType type, float price, int count);
}







