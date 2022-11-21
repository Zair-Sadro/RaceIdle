using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellSystem: MonoBehaviour
{

    [Zenject.Inject] private TileSetter _tileSetter;
    [Zenject.Inject] private StatsValuesInformator _statsValuesInformator;
    [SerializeField] private PlayerDetector _playerDetect;
    [SerializeField] private UIPanel _shopPanel;

    private Dictionary<TileType, TileEcoInfo> _priceByTypeDict = new Dictionary<TileType, TileEcoInfo>();


    public int TypesCount => _priceByTypeDict.Count;

    private void OnEnable()
    {
        _playerDetect.OnPlayerEnter += ShowOffer;
    }
    private void ShowOffer()
    {
        
        CalculateOffer();
        _shopPanel.Show();
        

    }

    private void CalculateOffer()
    {
        var dictionary = _tileSetter.TilesListsByType;

        var type = TileType.Junk;

        _priceByTypeDict.Clear();

        foreach ( var kvp in dictionary) 
        if (dictionary.TryGetValue(kvp.Key, out var t))
        {
            if (t.Count > 0)
            {
                TileEcoInfo tileEcoInfo = new TileEcoInfo(type,t.Count, CalculateTilePrice(type));
                _priceByTypeDict.Add(type, tileEcoInfo);

            }
        }
    }

    private float CalculateTilePrice(TileType type)
    {
        var price = _statsValuesInformator.GetMachineIncome(type)*0.3f;
       return price;
    }

    public IReadOnlyDictionary<TileType, TileEcoInfo> PriceInfo()
    {
        IReadOnlyDictionary<TileType, TileEcoInfo> dict = _priceByTypeDict;

        return dict;
    }


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
    [SerializeField] private SellSystem _sellSystem;
    [SerializeField] private List<ShopSlot> _sellslots;

    protected override void Start()
    {
        base.Start();
        PanelInit(GetPanelAnimation());

    }

    public override void Show()
    {
        RegisterSlots(_sellSystem.TypesCount);
        base.Show();
    }
    public override void Hide()
    {
        base.Hide();
        foreach (var slot in _sellslots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    private void RegisterSlots(int count)
    {
        var dict =_sellSystem.PriceInfo();
        var i = 0;
        foreach (var item in dict)
        {    i++;
            _sellslots[i].RegisterSlot(item.Value.type, item.Value.price, item.Value.count);
        }

       
    }

    [SerializeField, Tooltip("Скорость появления панели")]
    private float _showSpeed = 0.5f;
    private Tween GetPanelAnimation()
    {
        return transform
       .DOMoveY(0, _showSpeed)
       .SetEase(Ease.InOutFlash).Pause();
    }

    internal void RecieveSellInfo(List<TileList> lists)
    {
        throw new NotImplementedException();
    }
}

public class ShopSlot : MonoBehaviour
{

     [SerializeField] private TMP_Text _price;
     [SerializeField] private TMP_Text _priceForAll;
     [SerializeField] private TMP_Text _countTile;

     [SerializeField] private Button _sellAllButt;
     [SerializeField] private Button _sellAllOneButt;

     private TileType _type;

    public void RegisterSlot(TileType type,float price,int count)
    {
        _type = type;
        _countTile.text = count.ToString();
        _price.text = price.ToString();
        _priceForAll.text = (price * count).ToString();
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        _sellAllButt.onClick.AddListener(() => GameEventSystem.TileSold.Invoke(_type));
    }
    private void OnDisable()
    {
        _sellAllButt.onClick.RemoveAllListeners();
    }


}



