using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnObjectClickHelper : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public Action OnObjClick;
    bool _clicked;

    private void OnMouseDown()
    {
        _clicked = true;
    }

    public bool ClickedIndeed()
    {
        var c = _clicked;
        _clicked = false;
        return c;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _clicked = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _clicked = true;
    }
}
