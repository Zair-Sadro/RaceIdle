using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_HoldButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Button _button;

    public bool isHold = true;
    public bool isHoldDelay = false;
    public bool isHoldRealtime = false;


    public event Action myEvent;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private async void UpdateValue()
    {
        if (!isHoldDelay) return;
        while (isHoldDelay)
        {
            await Task.Delay(60);
            myEvent?.Invoke();
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if(!_button.enabled) return;
        
        if(!isHold) myEvent?.Invoke();
        
        isHoldRealtime = false;
        isHoldDelay = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!_button.enabled) return;
        
        isHoldRealtime = true;
        if (isHold)
        {
            myEvent?.Invoke();
            ActiveHold();
        }
    }

    private async void ActiveHold()
    {
        await Task.Delay(500);
        if (isHoldRealtime)
        {
            isHoldDelay = true;
            UpdateValue();
        }
    }
}
