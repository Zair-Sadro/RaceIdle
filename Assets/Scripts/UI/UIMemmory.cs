using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMemmory : MonoBehaviour
{
    [SerializeField] private UIController _lastUI;
    private Action <bool> _lastUIactions;
    public void ShowUI(UIController uIController, Action<bool> act=null)
    {
        act?.Invoke(true);
        _lastUIactions=act;
        uIController.Hide();
    }
    public void CloseUI()
    {
        _lastUIactions?.Invoke(false);
        _lastUI?.Hide();
        
    }
   
}
