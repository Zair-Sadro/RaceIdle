using System;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject _objectInScene;
    [SerializeField] private GameObject _upgradeObject;
    private IUpgradeMachine _machineUpgrade;

    [SerializeField] private UpgradeBar _upgradeBar;
    [SerializeField] private UpgradeSlot _incomeSlot;
    [SerializeField] private UpgradeSlot _speedCapacitySlot;
    private WalletSystem _wallet => InstantcesContainer.Instance.WalletSystem;


    private void Awake()
    {
        _machineUpgrade = _upgradeObject.GetInterface<IUpgradeMachine>();
    }
    private void Start()
    {

        _incomeSlot.InitUpgradeSlot(IncomeUpgrade);
        _speedCapacitySlot.InitUpgradeSlot(CapacitySpeedUpgrade);

        gameObject.SetActive(false);
    }

    public void Open()
    {
        _wallet.OnTotalMoneyChange += CheckPrice;
        CheckPrice(_wallet.TotalMoney);
        gameObject.SetActive(true);
        RefreshText();

    }
    public void Close()
    {
        _wallet.OnTotalMoneyChange -= CheckPrice;
        gameObject.SetActive(false);
    }

    public void CheckPrice(float total)
    {
        _incomeSlot.CanUpgrade(total);
        _speedCapacitySlot.CanUpgrade(total);
    }

    private void IncomeUpgrade()
    {
        UpgradeValues upgradeValues = _machineUpgrade.IncomeUpgradesFields.GetValues();
        float cost = upgradeValues.price;

        if (_wallet.TrySpendMoney(cost))
            _machineUpgrade.UpgradeIncome();

        upgradeValues = _machineUpgrade.IncomeUpgradesFields.GetValues();
        _incomeSlot.ChangeText(upgradeValues.price, upgradeValues.value);

        if (_machineUpgrade.IncomeUpgradesFields.MaxLevel)
            _speedCapacitySlot.MaxLevel();

    }
    private void CapacitySpeedUpgrade()
    {
        UpgradeValues upgradeValues = _machineUpgrade.SpeedUpgradeFields.GetValues();
        float cost = upgradeValues.price;

        if (_wallet.TrySpendMoney(cost))
            _machineUpgrade.UpgradeSpeedCapacity();

        upgradeValues = _machineUpgrade.SpeedUpgradeFields.GetValues();
        _speedCapacitySlot.ChangeText(upgradeValues.price, upgradeValues.value);
        if (_machineUpgrade.SpeedUpgradeFields.MaxLevel)
            _speedCapacitySlot.MaxLevel();
    }
    private void RefreshText()
    {
        UpgradeValues upgradeValues = _machineUpgrade.SpeedUpgradeFields.GetValues();
        _speedCapacitySlot.ChangeText(upgradeValues.price, upgradeValues.value);

        upgradeValues = _machineUpgrade.IncomeUpgradesFields.GetValues();
        _incomeSlot.ChangeText(upgradeValues.price, upgradeValues.value);
    }


}


