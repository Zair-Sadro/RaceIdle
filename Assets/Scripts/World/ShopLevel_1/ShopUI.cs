using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopUI : UIPanel 
{

    [SerializeField] private ShopSystem _sellSystem;
    [SerializeField] private List<GameObject> _slots;

    private List<IRegisterSlot> _registerSlots=new List<IRegisterSlot>();
    protected override void Awake()
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







