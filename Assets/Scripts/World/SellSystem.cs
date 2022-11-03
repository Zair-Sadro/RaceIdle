using System;
using System.Collections.Generic;
using UnityEngine;

public class SellSystem: MonoBehaviour
{

    [Zenject.Inject] private TileSetter _tileSetter;
    [SerializeField] private PlayerDetector _playerDetect;



    private void OnEnable()
    {
        _playerDetect.OnPlayerEnter += ShowOffer;
    }

    private void ShowOffer()
    {
        CalculateOffer();

    }

    private void CalculateOffer()
    {
        var v = _tileSetter.TilesListsByType;
    }
}

public class IncomeStatistic :MonoBehaviour
{
    [SerializeField] private List<TileMachine> machinesList;

    private SortedDictionary<TileType, MachineFields> machineFieldsByType = new SortedDictionary<TileType, MachineFields>();
    private void Start()
    {
        foreach (var item in machinesList)
        {
            machineFieldsByType.Add(item.typeProduced, item.machineFields);
        }
        
    }
    public float GetMachineIncome(TileType tileType)
    {
        return machineFieldsByType[tileType].Income;
    }
}

