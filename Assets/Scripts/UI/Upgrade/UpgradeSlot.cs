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

    public void InitUpgradeSlot(Action action)
    {
        _button.onClick.AddListener(() => action());
    }
    public void ChangeText(string price,string upgrade)
    {
        _priceText.text=price;
        _upgradeText.text=upgrade;

    }
}
