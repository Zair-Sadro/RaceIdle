using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    bool fingerUp = false;
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);

        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerDownHandler);
        ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerUpHandler);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        StopCoroutine(nameof(ClickWithNoJoystick));
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
        fingerUp = false;
       // StartCoroutine(ClickWithNoJoystick(eventData));
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        fingerUp = true;
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
    IEnumerator ClickWithNoJoystick(PointerEventData eventData)
    {
      //  yield return new WaitForSeconds(0.08f);

        if (fingerUp) 
        {

            yield break; 
        }

        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }
}