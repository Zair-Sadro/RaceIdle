using UnityEngine;

public class UIMemmory : MonoBehaviour
{
    [SerializeField] private UIController _lastUI;
    public void ShowUI(UIController uIController)
    {
        CloseUI();
        uIController.Show();
    }
    public void CloseUI()
    {
        _lastUI?.Hide();
        
    }
   
}
