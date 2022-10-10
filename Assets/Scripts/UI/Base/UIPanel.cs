using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : UIController
{
    protected CanvasGroup _canvasGrp;
    [Zenject.Inject] private UIMemmory _uIMemmory;

    protected void Open()
    {
        
        _uIMemmory.ShowUI(this, CloseOpenActions);
    }
    protected void Close()
    {
        _uIMemmory.ShowUI(this, CloseOpenActions);
    }

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
