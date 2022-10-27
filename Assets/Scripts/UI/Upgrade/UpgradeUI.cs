using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : UIPanel
{
    [SerializeField] private GameObject _objectInScene;
    [SerializeField] private MachineUpgrade _machineUpgrade;
    [SerializeField] private UpgradeSlot _incomeSlot;
    [SerializeField] private UpgradeSlot _speedCapacitySlot;
    
    [Space(3f),Header("Settings")]
    [SerializeField] private Button _closeButton;

    [SerializeField,Tooltip("Скорость появления панели")] 
    private float showSpeed=0.5f;

    protected override void Start()
    {
       
        base.Start();
        PanelInit(GetPanelAnimation());

       // _incomeSlot.InitUpgradeSlot(IncomeUpgrade);
     //   _speedCapacitySlot.InitUpgradeSlot(CapacitySpeedUpgrade);

        _closeButton.onClick.AddListener(() => Close());
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

        if (_objectInScene==obj)
        {
            Open();
        }
   
    }
    #endregion
    protected virtual void IncomeUpgrade()
    {
        // MoneyCount
        // if enough =>
        // _incomeSlot.ChangeText(,);
        // effects
        _machineUpgrade.UpgradeIncome();
        Debug.Log("Income UP");
    }
    protected virtual void CapacitySpeedUpgrade()
    {
        // MoneyCount
        // if enough =>
        _machineUpgrade.UpgradeSpeedCapacity(); 
        
        UpgradeValues upgradeValues = _machineUpgrade.Speed.GetValues();
        _speedCapacitySlot.ChangeText(upgradeValues.value.ToString(), upgradeValues.price.ToString());
        //effects

        Debug.Log("CapacitySpeed UP"); 
    }
    protected virtual Tween GetPanelAnimation()
    {

        return  transform
               .DOMoveY(0, showSpeed)
               .SetEase(Ease.InOutFlash).Pause() ;
    }

  
}


