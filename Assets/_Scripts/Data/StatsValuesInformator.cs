using System;
using System.Collections.Generic;
using UnityEngine;

//Создать отдельный класс откуда можно получить данные по машинам , цифрам и тд.
public class StatsValuesInformator :MonoBehaviour
{
    private BuildSaver _buildSaver => InstantcesContainer.Instance.BuildSaver;
    [SerializeField] private MachineFields _junkFields;

    private List<TileType> _inventedList = new();

    private void Start()
    {
        _inventedList.Add(TileType.Junk);
        machineFieldsByType.Add(TileType.Junk, _junkFields);
        foreach (var item in machinesList)
        {
            machineFieldsByType.Add(item.typeProduced, item.machineFields);
            item.TypeInvented += (tileType) => _inventedList.Add(tileType);
        }
        for (int i = 0; i <= _buildSaver.TileInvented; i++)
        {
            _inventedList.Add(StaticValues.TileGlobalIndex(i));
        }
    }
    #region TileMachines
    [SerializeField] private List<TileMachine> machinesList;

    private SortedList<TileType, MachineFields> machineFieldsByType
        = new SortedList<TileType, MachineFields>();
    public float GetMachineIncome(TileType tileType)
    {
        return machineFieldsByType[tileType].Income;
    }
    public List<TileType> GetMaxGainedType()
    {
        return _inventedList;
    }
    #endregion
}

