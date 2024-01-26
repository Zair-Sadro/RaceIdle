using UnityEngine;

public class WalletSystem : MonoBehaviour
{
    [SerializeField] private float _totalMoney=100000;

    public float TotalMoney => _totalMoney;
    public string TotalMoneyText => _totalMoney.ToString();

    public delegate void onTotalMoneyChange(float total);
    public event onTotalMoneyChange OnTotalMoneyChange;
    private void Start()
    {
        OnTotalMoneyChange?.Invoke(_totalMoney);
    }
    public bool TrySpendMoney(float amount)
    {
        if (amount > _totalMoney)
            return false;

        _totalMoney -= amount;
        OnTotalMoneyChange?.Invoke(_totalMoney);

        return true;
    }
    /// <summary>
    /// Return TRUE  if money is enough
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
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
    public void Init(float m)
    {
        _totalMoney = m;
    }
}
