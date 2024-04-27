using UnityEngine;


public class CarShop : MonoBehaviour
{
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private CarShopUI _shopPanel;
    private void OnEnable()
    {
        _playerDetector.OnPlayerEnter += ShowShop;
       
    }

    private void ShowShop()
    {
        _shopPanel.OpenPanel();
    }
}
