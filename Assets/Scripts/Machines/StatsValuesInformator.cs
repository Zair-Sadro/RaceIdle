using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
//Создать отдельный класс откуда можно получить данные по машинам , цифрам и тд.
public class StatsValuesInformator :MonoBehaviour
{
    private int _playerLevel;
    public int PlayerLevel => _playerLevel;
    private void Start()
    {
        foreach (var item in machinesList)
        {
            machineFieldsByType.Add(item.typeProduced, item.machineFields);
        }
        
    }
    private void OnEnable()
    {
        GameEventSystem.LevelUp += ((lvl) => _playerLevel = lvl);
    }

    #region TileMachines
    [SerializeField] private List<TileMachine> machinesList;

    private SortedDictionary<TileType, MachineFields> machineFieldsByType
        = new SortedDictionary<TileType, MachineFields>();
    public float GetMachineIncome(TileType tileType)
    {
        return machineFieldsByType[tileType].Income;
    }
    public List<TileType> GetMaxGainedType()
    {
        List<TileType> list = new List<TileType>();
        switch (_playerLevel)
        {
            case 1:
                AddType(TileType.Junk); 
                return list;

            case 2:
                AddType(TileType.Junk);
                AddType(TileType.Iron);
                return list; ;

            case 3:
                AddType(TileType.Junk);
                AddType(TileType.Iron);
                AddType(TileType.Plastic);
                return list;

            case 4:
                AddType(TileType.Junk);
                AddType(TileType.Iron);
                AddType(TileType.Plastic);
                AddType(TileType.Rubber);
                return list;

            case > 4:
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

