using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellSlot : MonoBehaviour, IRegisterSlot
{
     [SerializeField] private TMP_Text _priceText;
     [SerializeField] private TMP_Text _countTileText;

     [SerializeField] private Button _sellOneButt;

     [Zenject.Inject] private WalletSystem _wallet;

     private TileType _type;
     private float _price;
     private float _count;

    // same as constructor or Instantiate() slot with needed info 
    public void RegisterSlot(TileType type,float price,int count)
    {
        _type = type;
        _price = price;
        _count = count;

        _countTileText.text = count.ToString();
        _priceText.text = price.ToString();

        gameObject.SetActive(true);
    }

    #region Events
    private void OnEnable()
    {
        _sellOneButt.onClick.AddListener(() => OnSell());   
    }

    private void OnDisable()
    {
        _sellOneButt.onClick.RemoveAllListeners();
    }


    private void OnSell()
    {
        GameEventSystem.TileSold.Invoke(_type);
        --_count;
        _wallet.Income(_price);

        if (_count <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion


}







