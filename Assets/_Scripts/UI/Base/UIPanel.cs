using UnityEngine;


public class UIPanel : UIController
{
    [Zenject.Inject] protected UIMemmory UIMemmory;

    protected void Open()
    {
        
        UIMemmory.ShowUI(this);
    }
    protected void Close()
    {
        UIMemmory.CloseUI();
    }


}
