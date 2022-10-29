using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletSystem : MonoBehaviour
{
    [SerializeField] private float _totalMoney;

    public float TotalMoney => _totalMoney;
    public string TotalMoneyText => _totalMoney.ToString();

    public delegate void onTotalMoneyChange(float total);
    public event onTotalMoneyChange OnTotalMoneyChange;

    public bool TrySpendMoney(float amount)
    {
        if (amount > _totalMoney)
            return false;

        _totalMoney -= amount;
        OnTotalMoneyChange?.Invoke(_totalMoney);

        return true;
    }
    public bool CompareMoney(float amount)
    {
        if (amount > _totalMoney)
            return false;

        else 
            return true;

    }
    public void Income(float amount)
    {
        _totalMoney += amount;
        OnTotalMoneyChange?.Invoke(_totalMoney);

    }
}

public class ResourceView : MonoBehaviour
{
    [Zenject.Inject] private WalletSystem _wallet;

    [SerializeField] private TMPro.TMP_Text tx_money;
    private void OnEnable()
    {
        _wallet.OnTotalMoneyChange += ((total) => tx_money.text = total.ToString());
    }
    private void OnDisable()
    {
        _wallet.OnTotalMoneyChange -= ((total) => tx_money.text = total.ToString());
    }

   
}
