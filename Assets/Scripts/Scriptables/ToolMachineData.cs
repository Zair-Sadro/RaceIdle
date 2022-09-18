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
    [HideIf("_level", MachineLevelType.PreBuild), Tooltip("Time in sec")]
    [SerializeField] private float _createTime;
    [HideIf("_level", MachineLevelType.PreBuild)]
    [SerializeField] private int _tilePerCreateTime;
    [HideIf("_level", MachineLevelType.PreBuild)]
    [SerializeField] private int _maxTiles;
    [TableList, HideIf("_level", MachineLevelType.PreBuild)]
    [SerializeField] private List<ProductRequierment> _requierments = new List<ProductRequierment>();

    public MachineLevelType Level => _level;
    public float CreateTime => _createTime;
    public int TilePerCreateTime => _tilePerCreateTime;
    public int MaxTiles => _maxTiles;
    public List<ProductRequierment> Requierments => _requierments;


    [System.Serializable]

    public class ProductRequierment
    {
        [SerializeField] private TileType _type;
        [SerializeField] private int _amount;

        public TileType Type => _type;
        public int Amount => _amount;
    }

}
