using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarButton : MonoBehaviour
{
    [SerializeField] private Button _butt;

    [SerializeField] private float _cost;
    [SerializeField] private int _carIndex;
    [SerializeField] private TMP_Text _costText;

    [SerializeField] private WalletSystem _wallet;

    private void Start()
    {
        _butt.onClick.AddListener(BuyCar);
        _wallet.OnTotalMoneyChange += Reprice;

    }
    public void Init()
    {
        _costText.text = _cost.ToString();
    }

    private void BuyCar()
    {
        if (_wallet.TrySpendMoney(_cost))
        {
            GameEventSystem.OnCarBought.Invoke(_carIndex);
        }


    }
    private void Reprice(float walletMoney)
    {
        _butt.interactable = (_cost <= walletMoney);
    }
}
