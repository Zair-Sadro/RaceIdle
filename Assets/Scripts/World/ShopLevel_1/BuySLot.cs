using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySlot : MonoBehaviour, IRegisterSlot
{

    [SerializeField] private TMP_Text _priceText;

    [SerializeField] private Button _buyOneButt;

    [Zenject.Inject] private WalletSystem _wallet;
    [Zenject.Inject] private TileSetter _tileSetter;

    private TileType _type;
    private float _price;

    // same as constructor or Instantiate() slot with needed info 
    public void RegisterSlot(TileType type, float price, int count)
    {
        _type = type;
        _price = price;

        _priceText.text = price.ToString();

        Reprice(_wallet.TotalMoney);
        gameObject.SetActive(true);
    }

    #region Events
    private void OnEnable()
    {
        _wallet.OnTotalMoneyChange += Reprice;
        _buyOneButt.onClick.AddListener(() => OnBuy());
        _tileSetter.OnTilesMaxCapacity += OnMaxCount;
    }

    private void OnDisable()
    {
        _buyOneButt.onClick.RemoveAllListeners();
        _wallet.OnTotalMoneyChange -= Reprice;
        _tileSetter.OnTilesMaxCapacity -= OnMaxCount;
    }


    private void OnBuy()
    {
        GameEventSystem.TileBought.Invoke(_type);
    }


    private void Reprice(float walletMoney)
    {
        _buyOneButt.interactable = (_price <= walletMoney);
    }
    private void OnMaxCount(bool b)
    {
        _buyOneButt.interactable = false;
    }
    #endregion


}







