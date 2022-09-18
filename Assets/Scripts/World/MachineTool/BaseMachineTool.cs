﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class BaseMachineTool : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private ToolMachineData _machineData;
    [Space]
    [SerializeField] private List<MachineToolEntity> _machines = new List<MachineToolEntity>();

    private MachineToolEntity _currentActiveMachine;
    private List<MachineLevel.ProductRequierment> _currentRequierments;

    private List<Tile> _currentTilesInMachine = new List<Tile>();

    public event Action OnBuildZoneEnter;
    public event Action OnBuildZoneExit;

    public ToolMachineData MachineData => _machineData;
    public MachineToolEntity CurrentActiveMachine => _currentActiveMachine;

    [Inject] private TileSetter _tileSetter;

    private void Awake()
    {
        InitMachines();
    }

    private void Start()
    {
        CreateMachine(MachineLevelType.PreBuild);
    }

    private void InitMachines()
    {
        for (int i = 0; i < _machines.Count; i++)
            _machines[i].Init(this);
    }

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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if(requierments met)
            //   {
            //      Build/Upgrade();
            //      return;
            //   }
            //

            PlaceTiles();
        }
            
    }

    private void PlaceTiles()
    {
        _tileSetter.RemoveTiles(() => Debug.Log("Giving tiles"));

        for (int i = 0; i < _tileSetter.GivenTiles.Count; i++)
            _currentTilesInMachine.Add(_tileSetter.GivenTiles[i]);

        _tileSetter.GivenTiles.Clear();
    }

    private void CreateMachine(MachineLevelType level)
    {
        DisablePreviousMachine();
        FindNewMachine(level);
    }

    private void DisablePreviousMachine()
    {
        _currentActiveMachine?.gameObject.SetActive(false);
    }

    private void FindNewMachine(MachineLevelType level)
    {
        var machineLevelToBuild = _machines.Where(m => m.Level == level).FirstOrDefault();
        machineLevelToBuild.gameObject.SetActive(true);
        _currentActiveMachine = machineLevelToBuild;
        _currentRequierments = _machineData.GetNextMachineLevel(_currentActiveMachine.Level).Requierments;
    }
}
