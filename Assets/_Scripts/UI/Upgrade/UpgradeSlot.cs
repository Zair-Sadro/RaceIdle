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
        _button.GetComponent<UI_HoldButton>().myEvent += ()=> 
        {
            if (_button.interactable) action.Invoke(); 
        };
    }
    public void ChangeText(float price, float upgrade)
    {
        this.price = price;
        _priceText.text = price.ToString("0.##");
        _upgradeText.text = upgrade.ToString("0.##");

    }
    public void ChangeTextDirectly(float price,string text) 
    {
        this.price = price;
        _priceText.text = price.ToString("0.##");
        _upgradeText.text = text;
    }
    public void CanUpgrade(float walletMoney)
    {
        if (isMax)
            return;

        if (price <= walletMoney) _button.interactable = true;
        else _button.interactable = false;
    }
    private bool isMax;
    internal void MaxLevel()
    {
        isMax = true;
        _priceText.text = maximum;
        _button.interactable = false;

    }


    private string maximum;
    public void SubscribeToChange()
    {
        GameEventSystem.OnLanguageChange += ChangeLanguage;
    }

    public void ChangeLanguage(string key)
    {

        switch (key)
        {
            case "ru":
                maximum = "максимум";

                break;

            case "en":
                maximum = "max";

                break;
        }

        if (isMax) 
        {
            _priceText.text = maximum;
        }

    }
}
