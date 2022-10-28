using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : UIPanel
{
    [SerializeField] private GameObject _objectInScene;
    [SerializeField] private MachineUpgrade _machineUpgrade;
    [SerializeField] private UpgradeSlot _incomeSlot;
    [SerializeField] private UpgradeSlot _speedCapacitySlot;
    [Zenject.Inject] private WalletSystem _wallet;
    
    [Space(3f),Header("Settings")]
    [SerializeField] private Button _closeButton;

    [SerializeField,Tooltip("�������� ��������� ������")] 
    private float showSpeed = 0.5f;

    protected override void Start()
    {
       
        base.Start();
        PanelInit(GetPanelAnimation());

        _incomeSlot.InitUpgradeSlot(IncomeUpgrade);
        _speedCapacitySlot.InitUpgradeSlot(CapacitySpeedUpgrade);

        _closeButton.onClick.AddListener(() => Close());
    }


    #region Events
    protected virtual void OnEnable()
    {
        GameEventSystem.ObjectTaped += OnCLick;
        _wallet.OnTotalMoneyChange += CheckPrice;
    }
    protected virtual void OnDisable()
    {
        GameEventSystem.ObjectTaped -= OnCLick;
        _wallet.OnTotalMoneyChange -= CheckPrice;
    }
    public void OnCLick(GameObject obj)
    {
        if (_objectInScene == obj)
            Open();
    }
    public void CheckPrice(float total)
    {
        _incomeSlot.CanUpgrade(total);
        _speedCapacitySlot.CanUpgrade(total);
    }
    protected override void OnHide()
    {
        base.OnHide();
    } 
    #endregion

    protected virtual void IncomeUpgrade()
    {
        UpgradeValues upgradeValues = _machineUpgrade.Income.GetValues();
        float cost = upgradeValues.price;

        if(_wallet.CompareMoney(cost))
        _machineUpgrade.UpgradeIncome();

        upgradeValues = _machineUpgrade.Income.GetValues();
         _incomeSlot.ChangeText(upgradeValues.value, upgradeValues.price.ToString());
        // effects

        Debug.Log("Income UP");
    }
    protected virtual void CapacitySpeedUpgrade()
    {
        UpgradeValues upgradeValues = _machineUpgrade.Speed.GetValues();
        float cost = upgradeValues.price;

        if (_wallet.CompareMoney(cost))
            _machineUpgrade.UpgradeSpeedCapacity();

        upgradeValues = _machineUpgrade.Speed.GetValues();
        _speedCapacitySlot.ChangeText(upgradeValues.value, upgradeValues.price.ToString());
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


