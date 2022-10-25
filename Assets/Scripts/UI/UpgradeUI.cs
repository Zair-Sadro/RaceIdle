using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UpgradeUI : UIPanel
{
    [SerializeField] private GameObject _IUpgradeInScene;
    
    private bool IsClicked;
    protected override void OnEnable()
    {
        base.OnEnable();
        GameEventSystem.ObjectTaped += OnCLick;
    }
    private void OnDisable()
    {
        GameEventSystem.ObjectTaped -= OnCLick;
    }
    #region ClickEvent
    public void OnCLick(GameObject obj)
    {
        if (!IsClicked) return;

        if (obj == _IUpgradeInScene)
        {
            UIMemmory.ShowUI(this);
        }

        IsClicked = false;
    }
    private void OnMouseDown()
    {
        IsClicked = true;
    }
    #endregion
}
