using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MachineFields 
{

    [SerializeField] private TileType productType;
    [SerializeField] private List<ProductRequierment> _requierments = new List<ProductRequierment>();


    [Space(2f), Header("Settings")]

    [SerializeField] private  MachineNumbersData _incomeNumbers;
    [SerializeField] private  MachineNumbersData _speedNumbers;


    [SerializeField] private int[] levelsForCapUp;
    [SerializeField] private int _maxTiles;

    public int MaxTiles => _maxTiles;
    public float Speed { get; set; }
    public List<ProductRequierment> Requierments => _requierments;
    public TileType ProductType => productType;

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
        return levelsForCapUp;
    }
    public void CapacityUp(int newvalue)
    {
        _maxTiles = newvalue;
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

