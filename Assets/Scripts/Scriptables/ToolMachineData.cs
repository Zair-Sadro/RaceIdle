using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public enum MachineLevelType
{
    PreBuild,
    Level_1
}

[CreateAssetMenu(menuName = "Data/MachineData")]
public class ToolMachineData : ScriptableObject
{
    [SerializeField] private Tile _tileProduct;
    [SerializeField] private List<MachineLevel> _levels = new List<MachineLevel>();



    public Tile TileProduct => _tileProduct;
    public List<MachineLevel> Levels => _levels;
}

[System.Serializable]
public class MachineLevel
{
    [SerializeField] private TileType productType;
    [SerializeField] private List<ProductRequierment> _requierments = new List<ProductRequierment>();


    [Space(2f),Header("Settings")]

    [SerializeField] private float _createTime;
    [SerializeField] private float _delayMachineTakeTile;
    [SerializeField] private int _maxTiles;


  

    public float CreateTime => _createTime;
    public float DelayMachineTakeTile=> _delayMachineTakeTile;
    public int MaxTiles => _maxTiles;
    public List<ProductRequierment> Requierments => _requierments;

    public TileType ProductType => productType;

    public float startPrice { get; internal set; }
    internal float PriceFormula(float arg1, int arg2)
    {
        throw new NotImplementedException();
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
[System.Serializable]


public class MachineSettingsData : ScriptableObject
{
    public float startIncome;
    public float deltaIncome;
    public float startIncomePrice;
    public float deltaIncomePrice;
  
}
public enum OperationType
{
    Plus,
    Devide,
    Minus,
    Increase

}
