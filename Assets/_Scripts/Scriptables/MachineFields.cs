﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Machine/MachineFields")]
public class MachineFields:ScriptableObject
{

    [SerializeField] private TileType productType;
    [SerializeField] private List<ProductRequierment> _requierments = new List<ProductRequierment>();


    [Space(2f), Header("Settings")]

    [SerializeField] private  MachineNumbersData _incomeNumbers;
    [SerializeField] private  MachineNumbersData _speedNumbers;
    [SerializeField] private int _maxTiles;
    [SerializeField] private int[] levelsForCapacityUp;

    [SerializeField,Tooltip("Кэф прибавления вместимости")]
    private int _capacityDelta;


        //FIELDS\\
    public int MaxTiles => _maxTiles;
    public float Speed => _speedNumbers.currentValue;
    public float Income => _incomeNumbers.currentValue;
    public List<ProductRequierment> Requierments => _requierments;
    public TileType ProductType => productType;
    public int CapacityDelta => _capacityDelta;
 
    public void UpgradeSpeed(UpgradeField upgradeField)
    {
        _speedNumbers.currentValue=upgradeField.LevelUp();
    }
    public void UpgradeIncome(UpgradeField upgradeField)
    {
        _incomeNumbers.currentValue=upgradeField.LevelUp(); ;
    }
    public MachineNumbersData IncomeData()
    {
        return _incomeNumbers;
    }
    public MachineNumbersData SpeedData()
    {
        return _speedNumbers;
    }
    public int[] GetCapacityLevels()
    {
        return levelsForCapacityUp;
    }
    public void CapacityUp(int delta)
    {
        _maxTiles += delta;
    }
    
}

[System.Serializable]
public class ProductRequierment
{
    [SerializeField] private TileType _type;
    [SerializeField] private int _amount;

    public TileType Type => _type;
    public int Amount => _amount;
}

