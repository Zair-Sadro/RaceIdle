using System;
using UnityEngine;

public class CarRepairTask : MonoBehaviour, IGameTask
{
    [SerializeField] private FirstAutoRepair autoRepair;
    [SerializeField] private int count;
    [SerializeField] private int finalRepairLevelTarget;

    private int currentCount;
    public event Action TaskDone;

    public void EndTask()
    {
        autoRepair.OnCarRepairByPlayer -= CountCar;
    }

    public void StartTask()
    {
        if (autoRepair.RepairLevel == finalRepairLevelTarget|| !autoRepair.gameObject.activeSelf)
        {
            TaskDone.Invoke();
            return;
        }
           
        autoRepair.OnCarRepairByPlayer += CountCar;
    }

    private void CountCar()
    {
        ++currentCount;

        if(currentCount >= count) 
        {
            TaskDone?.Invoke();
        }
    }
}

