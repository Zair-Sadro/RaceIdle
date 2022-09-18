using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MachineTool : MonoBehaviour
{
    [SerializeField] private MachineLevelType _currentLevel;
    [SerializeField] private ToolMachineData _machineData;

    public event Action OnBuildZoneEnter;
    public event Action OnBuildZoneExit;

    public ToolMachineData MachineData => _machineData;
    public MachineLevelType CurrentLevel => _currentLevel;

    [Inject] private TileSetter _tileSetter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnBuildZoneEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            OnBuildZoneExit?.Invoke();
    }

}
