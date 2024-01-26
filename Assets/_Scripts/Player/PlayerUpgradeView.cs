using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PlayerUpgradeView : UIPanel
{
    [SerializeField] private UpgradeSlot _speedSlot, _damageSlot, _capacitySlot;
    [SerializeField] private WalletSystem _wallet;

    [SerializeField] private TMP_Text _currentDamage, _currentSpeed, _currentCapacity;

    [SerializeField] private Button _exitButt;
    [SerializeField] private Button _openButt;

    private UpgradeNumbersData SpeedData;
    private UpgradeNumbersData DamageData;
    private UpgradeNumbersData CapacityData;
    protected override void Awake()
    {
        base.Awake();
        PanelInit(GetPanelAnimation());

        OnPanelShow += SubscribeEvents;
        OnPanelHide += UnSubscribeEvents;

        _exitButt.onClick.AddListener(Close);
        _openButt.onClick.AddListener(Open);
        _openButt.onClick.AddListener(()=>CheckPrice(_wallet.TotalMoney));


    }
    private void SubscribeEvents()
    {
        _wallet.OnTotalMoneyChange += CheckPrice;
    }
    private void UnSubscribeEvents()
    {
        _wallet.OnTotalMoneyChange -= CheckPrice;
    }
    private void CheckPrice(float total)
    {
        _speedSlot.CanUpgrade(total);
        _damageSlot.CanUpgrade(total);
        _capacitySlot.CanUpgrade(total);
    }

    public void InitSpeed(UpgradeNumbersData data, Action action)
    {
        SpeedData = data;
        _currentSpeed.text = ((int)data.currentValue).ToString();

        if (data.IsMaxLevel) 
        {
            _speedSlot.ChangeTextDirectly(0, "");
            _speedSlot.MaxLevel();
            return;
        }
        else 
        {
            _speedSlot.ChangeTextDirectly(data.currentPriceValue, $"+{data.DeltaNumber}");

            Action upgradeAction = CreateUpgradeAction(action, SpeedData, _speedSlot, _currentSpeed);
            _speedSlot.InitUpgradeSlot(upgradeAction);
        }



    }
    public void InitDamage(UpgradeNumbersData data, Action action)
    {
        DamageData = data;
        _currentDamage.text = ((int)data.currentValue).ToString();


        if (data.IsMaxLevel)
        {
            _damageSlot.ChangeTextDirectly(0, "");
            _damageSlot.MaxLevel();
            return;
        }
        else
        {
            _damageSlot.ChangeTextDirectly(data.currentPriceValue, $"+{data.DeltaNumber}");

            Action upgradeAction = CreateUpgradeAction(action, DamageData, _damageSlot, _currentDamage);
            _damageSlot.InitUpgradeSlot(upgradeAction);
        }



    }

    public void InitCapacity(UpgradeNumbersData data, Action action)
    {
        CapacityData = data;
        _currentCapacity.text = ((int)data.currentValue).ToString();

        if (data.IsMaxLevel)
        {
            _capacitySlot.ChangeTextDirectly(0, "");
            _capacitySlot.MaxLevel();
            return;
        }
        else
        {
            _capacitySlot.ChangeTextDirectly(data.currentPriceValue, $"+{data.DeltaNumber}");

            Action upgradeAction = CreateUpgradeAction(action, CapacityData, _capacitySlot, _currentCapacity);
            _capacitySlot.InitUpgradeSlot(upgradeAction);
        }

    }
    private Action CreateUpgradeAction(Action action, UpgradeNumbersData data, UpgradeSlot upgradeSlot,TMP_Text txt)
    {
        return (() =>
        {
            _wallet.TrySpendMoney(data.currentPriceValue);
            action.Invoke();
            txt.text = ((int)data.currentValue).ToString();
            if (data.IsMaxLevel)
                upgradeSlot.MaxLevel();
            else
                upgradeSlot.ChangeTextDirectly(data.currentPriceValue, $"+{data.DeltaNumber}");


        });
    }


    [SerializeField, Tooltip("Скорость появления панели")]
    private float _showSpeed = 0.5f;
    private Tween GetPanelAnimation()
    {
        return s_canvasGroup.DOFade(1, _showSpeed).Pause();

    }

}
