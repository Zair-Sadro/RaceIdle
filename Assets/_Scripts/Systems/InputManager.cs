using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private float startTime = 0;

    public bool onGUI = false;

    private RaycastHit _raycastHit;
    private List<RaycastResult> uiRaycastResults = new List<RaycastResult>();
    private Touch _touch;
    private Camera _camera;
    private Ray _ray;

    public void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (InstantcesContainer.Instance.UIMemmory.InUI) return;

        if (Input.touchCount > 0)
        {

            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
                startTime = Time.time;

            if (Time.time - startTime <= 0.2f &&
                _touch.phase == TouchPhase.Ended)
            {

                if (IsPointerOverUIObject()) return;

                _ray = _camera.ScreenPointToRay
                    (new Vector3(_touch.position.x, _touch.position.y, 0));

                if (Physics.Raycast(_ray, out _raycastHit, Mathf.Infinity))
                {
                    if (_raycastHit.collider && _raycastHit.transform.tag == "UIOpenable")
                    {
                        GameEventSystem.ObjectTaped?.Invoke(_raycastHit.collider.gameObject);

                    }
                }
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = _touch.position;

        uiRaycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, uiRaycastResults);

        if (uiRaycastResults.Count == 1)
        {
            if (uiRaycastResults[0].gameObject.tag == "Joystick")
                return false;
        }
        return uiRaycastResults.Count > 1;
    }

    public void DisableRaycast()
    {
        onGUI = true;
        //closeHandler.gameObject.SetActive(true);
    }

    public void EnableRaycast()
    {
        onGUI = false;
        //closeHandler.gameObject.SetActive(false);
    }

    public void Close()
    {
        Application.Quit();
    }

}
