using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : UIController
{
    protected CanvasGroup _canvasGrp;
    [Zenject.Inject] protected UIMemmory UIMemmory;

    protected virtual void Start()
    {
        _canvasGrp = GetComponent<CanvasGroup>();

    }
    protected void Open()
    {
        
        UIMemmory.ShowUI(this);
    }
    protected void Close()
    {
        UIMemmory.CloseUI();
    }


}
