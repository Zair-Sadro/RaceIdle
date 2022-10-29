using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [Zenject.Inject] private WalletSystem _wallet;

    [SerializeField] private TMPro.TMP_Text _moneyText;
    private void OnEnable()
    {
        _wallet.OnTotalMoneyChange += MoneyView;
    }
    private void MoneyView(float total)
    {
        _moneyText.text = total.ToString();
    }
}
