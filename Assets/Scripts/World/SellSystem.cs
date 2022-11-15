using System;
using System.Collections.Generic;
using UnityEngine;

public class SellSystem: MonoBehaviour
{

    [Zenject.Inject] private TileSetter _tileSetter;
    [SerializeField] private PlayerDetector _playerDetect;
    [SerializeField] private UIPanel _shopPanel;


    private void OnEnable()
    {
        _playerDetect.OnPlayerEnter += ShowOffer;
    }

    private void ShowOffer()
    {
        CalculateOffer();
        _shopPanel.Show();

    }

    private void CalculateOffer()
    {
        var v = _tileSetter.TilesListsByType;
    }
}

//Создать отдельный класс откуда можно получить данные по машинам , цифрам и тд.
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

