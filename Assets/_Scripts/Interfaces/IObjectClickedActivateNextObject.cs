using System;

public interface IObjectClickedActivateNextObject
{
    public event Action<int> OnObjectClicked;
    public int ObjId { get; }
}


