using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopUI<T> : UIPanel where T : IRegisterSlot
{

    [SerializeField] private ShopSystem _sellSystem;
    [SerializeField] private List<GameObject> _slots;
    [SerializeField] private Component slotType;


    private List<T> _registerSlots;
    protected override void Awake()
    {
        base.Awake();
        GetIRegister();
        PanelInit(GetPanelAnimation());

    }


    private void GetIRegister()
    {
        if (_slots[0].TryGetComponent<SellSlot>(out var slot)) 
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                _registerSlots[i] = _slots[0].GetComponent<T>();
            }
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
        return transform
       .DOScale(Vector3.one, _showSpeed)
       .SetEase(Ease.InOutFlash).Pause();
    }
  
    #endregion
}







