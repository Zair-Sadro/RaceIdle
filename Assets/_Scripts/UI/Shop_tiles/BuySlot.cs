using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySlot : MonoBehaviour, IRegisterSlot
{

    [SerializeField] private TMP_Text _priceText;

    [SerializeField] private Button _buyOneButt;

    private WalletSystem _wallet => InstantcesContainer.Instance.WalletSystem;
    private TileSetter _tileSetter => InstantcesContainer.Instance.TileSetter;

    private TileType _type;
    private float _price;


    public void RegisterSlot(TileType type, float price, int count)
    {
        _type = type;
        _price = price;

        _priceText.text = price.ToString();

        Reprice(_wallet.TotalMoney);
        gameObject.SetActive(true);
    }

    #region Events
    void Start()
    {
        _wallet.OnTotalMoneyChange += Reprice;
        _buyOneButt.onClick.AddListener(() => OnBuy());
        _tileSetter.OnTilesMaxCapacity += OnMaxCount;
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







