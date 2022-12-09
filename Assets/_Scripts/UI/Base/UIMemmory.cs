using UnityEngine;

public class UIMemmory : MonoBehaviour
{
    [SerializeField] private UIController _lastUI;
    public void ShowUI(UIController uIController)
    {
        CloseUI();
        uIController.Show();
        _lastUI = uIController;
    }
    public void CloseUI()
    {
        _lastUI?.Hide();
        
    }
   
}
