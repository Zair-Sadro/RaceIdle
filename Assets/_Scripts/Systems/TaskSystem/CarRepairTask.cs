﻿using System;
using UnityEngine;

public class CarRepairTask : MonoBehaviour, IGameTask
{
    [SerializeField] private AutoRepair autoRepair;
    [SerializeField] private int count;

    private int currentCount;
    public event Action TaskDone;

    public void EndTask()
    {
        autoRepair.OnCarRepairByPlayer -= CountCar;
    }

    public void StartTask()
    {
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

