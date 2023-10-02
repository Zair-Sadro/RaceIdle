using UnityEngine;


public class UIPanel : UIController
{
     protected UIMemmory UIMemmory=>InstantcesContainer.Instance.UIMemmory;

    protected void Open()
    {
        
        UIMemmory.ShowUI(this);
    }
    protected void Close()
    {
        UIMemmory.CloseUI();
    }


}
