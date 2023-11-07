using UnityEngine;

public class UIMemmory : MonoBehaviour
{
    [SerializeField] private UIController _lastUI;
    private bool _inUI;
    public bool InUI => _inUI;
    public void ShowUI(UIController uIController)
    {        
        CloseUI();

        _inUI = true;
        uIController.Show();
        _lastUI = uIController;
    }
    public void CloseUI()
    {
        _inUI = false;
        _lastUI?.Hide();
        
    }
   
}
