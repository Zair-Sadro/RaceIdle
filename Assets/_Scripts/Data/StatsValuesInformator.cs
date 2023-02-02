using System.Collections.Generic;
using UnityEngine;

//Создать отдельный класс откуда можно получить данные по машинам , цифрам и тд.
public class StatsValuesInformator :MonoBehaviour
{
    [Zenject.Inject] private BuildSaver _buildSaver;
    [SerializeField] private MachineFields _junkFields;
    private void Start()
    {
        machineFieldsByType.Add(TileType.Junk, _junkFields);
        foreach (var item in machinesList)
        {
            machineFieldsByType.Add(item.typeProduced, item.machineFields);
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
        List<TileType> list = new List<TileType>();
        switch (_buildSaver.TileInvented)
        {
            case 0:
                AddType(TileType.Junk); 
                return list;

            case 1:
                AddType(TileType.Junk);
                AddType(TileType.Iron);
                return list; ;

            case 2:
                AddType(TileType.Junk);
                AddType(TileType.Iron);
                AddType(TileType.Plastic);
                return list;

            case > 2:
                AddType(TileType.Junk);
                AddType(TileType.Iron);
                AddType(TileType.Plastic);
                AddType(TileType.Rubber);
                return list;

            default:
                AddType(TileType.Junk);
                return list;
                
        }

        void AddType(TileType type)
        {
            list.Add(type);
        }
    }
    #endregion
}

