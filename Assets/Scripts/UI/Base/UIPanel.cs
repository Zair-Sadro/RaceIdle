using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : UIController
{
    protected CanvasGroup _canvasGrp;
    [Zenject.Inject] protected UIMemmory UIMemmory;

    protected void Open()
    {
        
        UIMemmory.ShowUI(this, CloseOpenActions);
    }
    protected void Close()
    {
        UIMemmory.ShowUI(this, CloseOpenActions);
    }

    // additional action if need
    protected virtual void CloseOpenActions(bool open)
    {
        //Example
        if (open)
        {

        }
        else
        {

        }

        void Open()
        {
            
        }
        void Close()
        {

        }
    }
    void OnStartInit()
    {
        _canvasGrp = GetComponent<CanvasGroup>();
    }

}
