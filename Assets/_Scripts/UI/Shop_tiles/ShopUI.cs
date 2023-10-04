using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ShopUI : UIPanel 
{

    [SerializeField] private ShopSystem _sellSystem;
    [SerializeField] private List<GameObject> _slots;


    private List<IRegisterSlot> _registerSlots = new List<IRegisterSlot>();

    protected override void Start()
    {
        base.Awake();
        GetIRegister();
        PanelInit(GetPanelAnimation());
    }


    private void GetIRegister()
    {
      
       for (int i = 0; i < _slots.Count; i++)
       {
          var ireg = _slots[i].GetComponent(typeof(IRegisterSlot)) as IRegisterSlot;
         _registerSlots.Add(ireg);
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
        {   
            _registerSlots[i].RegisterSlot(item.Value.type, item.Value.price, item.Value.count);
            i++;
        }
       
    }
    #region PanelAnimations

    [SerializeField, Tooltip("Скорость появления панели")]
    private float _showSpeed = 0.5f;
    private Tween GetPanelAnimation()
    {
        return s_canvasGroup.DOFade(1, _showSpeed).Pause();

    }
  
    #endregion
}

public class SellAllButton : MonoBehaviour
{
    private WalletSystem _wallet => InstantcesContainer.Instance.WalletSystem;

    [SerializeField] private Button _button;
    [SerializeField] private ShopSystem _shopSystem;
    [SerializeField] private ShopUI _sellShop;
    [SerializeField] private List<SellSlot> _sellSlots;

    [SerializeField] private TMPro.TMP_Text _priceText;
    private void OnEnable()
    {
        _sellShop.OnPanelShow += OnButtonVisible;
        GameEventSystem.TileSold += ((s) => Reprice());
    }
    private void OnDisable()
    {
        _sellShop.OnPanelShow -= OnButtonVisible;
        GameEventSystem.TileSold -= ((s) => Reprice());
    }
    private void OnButtonVisible()
    {
        Reprice();
    }
    private void Reprice()
    {
        float price = 0;
        foreach (var item in _sellSlots)
        {
            price += item.AllPrice;
        }
        _priceText.text = price.ToString();

       _button.interactable = _wallet.CompareMoney(price);
    }
    
}







