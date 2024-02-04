using System.Collections.Generic;
using UnityEngine;

//Создать отдельный класс откуда можно получить данные по машинам , цифрам и тд.
public class StatsValuesInformator : MonoBehaviour
{
    [SerializeField] private MachineFields _junkFields;

    private List<TileType> _inventedList = new();
    private List<int> _machinesIndexex = new();
    private void Awake()
    {
        _inventedList.Add(TileType.Junk);
        machineFieldsByType.Add(TileType.Junk, _junkFields);
       // _machinesIndexex.Add(0);

        int machineIndx = 0;
        foreach (var item in machinesList)
        {
            machineFieldsByType.Add(item.typeProduced, item.machineFields);
            item.TypeInvented += (tileType) =>
            {
                _inventedList.Add(tileType);
                _machinesIndexex.Add(machineIndx++);
            };

        }

        foreach (var item in autoMachineList)
        {
            autoMachineFieldsByType.Add(item.typeProduced, item.ProducerFields);
            item.TypeInvented += (tileType) =>
            {
                _machinesIndexex.Add(machineIndx++);
            };

        }

    }
    #region TileMachines
    [SerializeField] private List<TileMachine> machinesList;
    [SerializeField] private List<TileProducerMachine> autoMachineList;

    private SortedList<TileType, MachineFields> machineFieldsByType = new();

    private SortedList<TileType, ProducerFields> autoMachineFieldsByType = new();

    public float GetMachineIncome(TileType tileType)
    {
        return machineFieldsByType[tileType].Income;
    }
    public float GetAutoMachineIncome(TileType tileType)
    {
        return autoMachineFieldsByType[tileType].Income;
    }
    public List<TileType> GetMaxGainedType()
    {
        return _inventedList;
    }
    public List<int> GetBuiltMachines()
    {
        return _machinesIndexex;
    }
    #endregion
    public void ResetValues()
    {
        foreach (var item in machinesList)
        {
            item.machineFields.ToDefault();
        }
        foreach (var item in autoMachineList)
        {
            item.ProducerFields.ToDefault();
        }
    }
}

