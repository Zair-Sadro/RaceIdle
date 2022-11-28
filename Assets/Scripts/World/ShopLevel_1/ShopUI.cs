using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : UIPanel
{
    [SerializeField] private ShopSystem _sellSystem;
    [SerializeField] private List<GameObject> _slots;


    private List<IRegisterSlot> _registerSlots;
  
    private void Start()
    {

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
       .DOScale(Vector3.one, _showSpeed)
       .SetEase(Ease.InOutFlash).Pause();
    }
    #endregion
}







