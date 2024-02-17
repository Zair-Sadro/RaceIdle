using UnityEngine;

[CreateAssetMenu(menuName = "Machine/ProducerFields")]
public class ProducerFields : ScriptableObject
{
    [SerializeField] private TileType productType;

    [Space(2f), Header("Settings")]

    [SerializeField] private UpgradeNumbersData _incomeNumbers;
    [SerializeField] private UpgradeNumbersData _speedNumbers;
    [SerializeField] private int _maxTiles = 4;
    [SerializeField] private int[] levelsForCapacityUp;
    [SerializeField, Tooltip("Кэф прибавления вместимости")]
    private int _capacityDelta;

    //FIELDS\\
    public int MaxTiles => _maxTiles;
    public float Speed => _speedNumbers.currentValue;
    public float Income => _incomeNumbers.currentValue;
    public TileType ProductType => productType;
    public int CapacityDelta => _capacityDelta;

    public void UpgradeSpeed(UpgradeField upgradeField)
    {
        _speedNumbers.currentValue = upgradeField.LevelUp();
        _speedNumbers.currentLevel = upgradeField.Level;
        _speedNumbers.currentPriceValue = upgradeField.CurrentPrice;
    }
    public void UpgradeIncome(UpgradeField upgradeField)
    {
        _incomeNumbers.currentValue = upgradeField.LevelUp();
        _incomeNumbers.currentPriceValue = upgradeField.CurrentPrice;
    }
    public UpgradeNumbersData IncomeData()
    {
        return _incomeNumbers;
    }
    public UpgradeNumbersData SpeedData()
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
    public void SetCapacity(int capacity)
    {
        _maxTiles = capacity;
    }
    public void ToDefault()
    {
        _speedNumbers.currentValue = _speedNumbers.startNumber;
        _incomeNumbers.currentValue = _incomeNumbers.startNumber;

        _speedNumbers.currentPriceValue = _speedNumbers.startNumberPrice;
        _incomeNumbers.currentPriceValue = _incomeNumbers.startNumberPrice;

        _speedNumbers.currentLevel = 0;
        _incomeNumbers.currentLevel = 0;
    }
}

