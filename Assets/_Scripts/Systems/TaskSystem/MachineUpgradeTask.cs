using System;
using UnityEngine;

public class MachineUpgradeTask : MonoBehaviour, IGameTask
{
    [SerializeField] private MachineUpgrade machine;

    public event Action TaskDone;

    public void EndTask()
    {
        machine.OnIncomeUpgraded -= (s) => TaskDone?.Invoke();
        machine.OnSpeedUpgraded -= (s) => TaskDone?.Invoke();
    }

    public void StartTask()
    {
        machine.OnIncomeUpgraded += (s)=>TaskDone?.Invoke();
        machine.OnSpeedUpgraded += (s) => TaskDone?.Invoke();

    }

}

