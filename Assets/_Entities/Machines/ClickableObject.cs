using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IObjectClickedActivateNextObject
{

    [SerializeField] private int _objIDForClick;
    public event Action<int> OnObjectClicked;

    private bool _downOnObject;
    private UIMemmory _UIMemmory => InstantcesContainer.Instance.UIMemmory;
    public int ObjId => _objIDForClick;

    private void Awake()
    {
        GameEventSystem.ObjectTaped += CheckGameObjAndRiseEvent;
    }
    public void CheckGameObjAndRiseEvent(GameObject go)
    {
        if (go == this.gameObject)
        {
            OnObjectClicked?.Invoke(_objIDForClick);
        }
    }
    private void OnMouseDown()
    {
        _downOnObject = true;
        Debug.Log("Down");
    }
    private void OnMouseExit()
    {
        _downOnObject = false;
    }
    private void OnMouseUp()
    {
        if (_downOnObject && !_UIMemmory.InUI)
        {
            Debug.Log("Up");
            OnObjectClicked?.Invoke(_objIDForClick);
        }
    }

}


