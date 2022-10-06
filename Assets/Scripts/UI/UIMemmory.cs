using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMemmory : MonoBehaviour
{
    [SerializeField] private UIController _lastUI;

    public void ShowUI(UIController uIController)
    {
        
    }
    public void CloseUI()
    {
        _lastUI?.Hide();
        
    }
   
}
public class UIPanelDefender : UIController
{
    //Makes raycasts to any panels safe - no bug , no unnecessery open or close 
    

   
}
