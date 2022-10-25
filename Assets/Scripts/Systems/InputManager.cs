using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager: MonoBehaviour //: GenericSingletonClass<InputManager>
{
    private static InputManager instance;
    public static InputManager Instance => instance;
    private float startTime = 0;

    public bool onGUI = false;

    private RaycastHit _raycastHit;
    private List<RaycastResult> uiRaycastResults = new List<RaycastResult>();
    private Touch _touch;
    private Camera _camera;
    private Ray _ray;

    private bool This_is_UI;


    public void Awake()
    {       
        _camera = Camera.main;
    }
    
    private void FixedUpdate()
    {
        if(onGUI) return;
        
        if (Input.touchCount > 0)
        {
            
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
                startTime = Time.time;

            if (Time.time - startTime <= 0.6f &&
                _touch.phase == TouchPhase.Ended)
            {
                //провряем задели ли UI
                if (IsPointerOverUIObject()) return;
                
                
                //иначе проверяем коллайдер и вызываем событие
                _ray = _camera.ScreenPointToRay(new Vector3(_touch.position.x, _touch.position.y, 0));
                if (Physics.Raycast(_ray, out _raycastHit, Mathf.Infinity))
                {
                    if (_raycastHit.collider)
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
        return uiRaycastResults.Count > 0;
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
