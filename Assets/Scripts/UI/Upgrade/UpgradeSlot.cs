using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UpgradeSlot : MonoBehaviour
{

    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _upgradeText;

    private float price;

    public void InitUpgradeSlot(Action action)
    {
        _button.onClick.AddListener(() => action());
    }
    public void ChangeText(float price,float upgrade)
    {
        this.price = price;
        _priceText.text = price.ToString("0.##");
        _upgradeText.text = upgrade.ToString("0.##");

    }
    public void CanUpgrade(float walletMoney)
    {
        if (price <= walletMoney) _button.interactable = true;
        else _button.interactable = false;
    }
}
