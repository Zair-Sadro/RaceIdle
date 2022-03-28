using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTool : MonoBehaviour
{
    [SerializeField] private MachineLevelType _currentLevel;
    [SerializeField] private ToolMachineData _machineData;

    public event Action OnBuildZoneEnter;
    public event Action OnBuildZoneExit;

    public ToolMachineData MachineData => _machineData;
    public MachineLevelType CurrentLevel => _currentLevel;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TileSetter player))
            OnBuildZoneEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out TileSetter player))
            OnBuildZoneExit?.Invoke();
    }

}
