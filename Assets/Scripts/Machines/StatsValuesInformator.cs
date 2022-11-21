using System.Collections.Generic;
using UnityEngine;
//Создать отдельный класс откуда можно получить данные по машинам , цифрам и тд.
public class StatsValuesInformator :MonoBehaviour
{

    private void Start()
    {
        foreach (var item in machinesList)
        {
            machineFieldsByType.Add(item.typeProduced, item.machineFields);
        }
        
    }

    #region TileMachines
    [SerializeField] private List<TileMachine> machinesList;

    private SortedDictionary<TileType, MachineFields> machineFieldsByType
        = new SortedDictionary<TileType, MachineFields>();
    public float GetMachineIncome(TileType tileType)
    {
        return machineFieldsByType[tileType].Income;
    }
    #endregion
}

