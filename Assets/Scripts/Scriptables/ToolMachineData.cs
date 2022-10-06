using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

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


    public MachineLevel GetMachineLevel(MachineLevelType lvl)
    {
        return _levels.Where(m => m.Level == lvl).FirstOrDefault();
    }

    public MachineLevel GetNextMachineLevel(MachineLevelType currentLvl)
    {
        return _levels.Where(m => m.Level == currentLvl + 1).FirstOrDefault();
    }

    public Tile TileProduct => _tileProduct;
    public List<MachineLevel> Levels => _levels;
}

[System.Serializable]
public class MachineLevel
{
    [SerializeField] private MachineLevelType _level;
    [SerializeField] private TileType productType;
    [SerializeField] private float _createTime;
    [SerializeField] private float _delayMachineTakeTile;
    [SerializeField] private int _maxTiles;
    [SerializeField] private List<ProductRequierment> _requierments = new List<ProductRequierment>();



    public MachineLevelType Level => _level;
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
