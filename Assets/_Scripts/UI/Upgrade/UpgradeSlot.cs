using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour, ILanguageChange
{

    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _upgradeText;

    private float price;

    private void Awake()
    {
        SubscribeToChange();
    }
    public void InitUpgradeSlot(Action action)
    {
        _button.GetComponent<UI_HoldButton>().myEvent += action;
    }
    public void ChangeText(float price, float upgrade)
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

    internal void MaxLevel()
    {
        _button.interactable = false;
        _priceText.text = maximum;
    }


    private string maximum;
    [SerializeField] private string ru = "максимум";
    [SerializeField] private string en;
    public void SubscribeToChange()
    {
        GameEventSystem.OnLanguageChange += ChangeLanguage;
    }

    public void ChangeLanguage(string key)
    {
        switch (key)
        {
            case "ru":
                maximum = ru;

                break;

            case "en":
                maximum = en;

                break;
        }

    }
}
