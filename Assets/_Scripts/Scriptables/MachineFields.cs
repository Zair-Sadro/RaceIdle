using System.Collections.Generic;
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



    private float _income;
    private float _speed;

        //FIELDS\\
    public int MaxTiles => _maxTiles;
    public float Speed => _speed;
    public float Income => _income;
    public List<ProductRequierment> Requierments => _requierments;
    public TileType ProductType => productType;
    public int CapacityDelta => _capacityDelta;
 
    public void UpgradeSpeed(UpgradeField upgradeField)
    {
        _speed = upgradeField.LevelUp();
    }
    public void UpgradeIncome(UpgradeField upgradeField)
    {
        _income = upgradeField.LevelUp(); ;
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

