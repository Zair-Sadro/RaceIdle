using DG.Tweening;
using UnityEngine;

public class UpgradeUI : UIPanel
{
    [SerializeField] private OnObjectClickHelper _IUpgradeInScene;
    [SerializeField] private TileMachine _machine;
    [SerializeField] private UpgradeSlot _incomeSlot;
    [SerializeField] private UpgradeSlot _speedCapacitySlot;

    protected override void Start()
    {
       
        base.Start();
        PanelInit(GetPanelAnimation());

        _incomeSlot.InitUpgradeSlot(IncomeUpgrade);
        _speedCapacitySlot.InitUpgradeSlot(CapacitySpeedUpgrade);
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    protected virtual void OnEnable()
    {
        GameEventSystem.ObjectTaped += OnCLick;
    }
    protected virtual void OnDisable()
    {
        GameEventSystem.ObjectTaped -= OnCLick;
    }
    #region ClickEvent
    public void OnCLick(GameObject obj)
    {

        if (_IUpgradeInScene.ClickedIndeed())
        {
            UIMemmory.ShowUI(this);
        }
   
    }
    #endregion
    protected virtual void IncomeUpgrade()
    {
        // MoneyCount
        // if enough =>
        // _incomeSlot.ChangeText(,);
        // effects
        _machine.UpgradeIncome();
        Debug.Log("Income UP");
    }
    protected virtual void CapacitySpeedUpgrade()
    {
        // MoneyCount
        // if enough =>
        // _speedCapacitySlot.ChangeText(,);
        // effects
        _machine.UpgradeSpeedCapacity();
        Debug.Log("CapacitySpeed UP"); 
    }
    protected virtual Tween GetPanelAnimation()
    {

        return  transform
               .DOMoveY(0, 1f)
               .SetEase(Ease.InElastic).Pause() ;
    } 
}
