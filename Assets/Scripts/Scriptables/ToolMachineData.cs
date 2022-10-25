using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
    [SerializeField] private float _createTime;
    [SerializeField] private float _delayMachineTakeTile;
    [SerializeField] private int _maxTiles;
    [SerializeField] private List<ProductRequierment> _requierments = new List<ProductRequierment>();

    public float CreateTime => _createTime;
    public float DelayMachineTakeTile=> _delayMachineTakeTile;
    public int MaxTiles => _maxTiles;
    public List<ProductRequierment> Requierments => _requierments;

    public TileType ProductType => productType;

 

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
public class UpgradeLevel
{
    public int level;
    public int maxLevel;

    public int startValue;
    public float delta;
}
[System.Serializable]
public class MachineData
{
    public UpgradeLevel speedLevel, capacityLevel; 
}

[System.Serializable]
public enum OperationType
{
    Plus,
    Devide,
    Minus,
    Increase

}
