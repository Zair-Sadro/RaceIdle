using System;
using TMPro;
using UnityEngine;

public class MachineUpgrade : MonoBehaviour, ISaveLoad<MachineUpgradeData>, IUpgradeMachine
{
    [SerializeField] private Mesh[] upgradeMeshes;
    [SerializeField] private MeshFilter meshFilter;

    [SerializeField] private MachineFields _machineBaseFields;

    [SerializeField] private TMP_Text _capacityValueText, _speedValueText, _incomeValueText;

    private int[] capacityUpLevels;
    private int indexer;

    private UpgradeField _speedUpgradesFields;
    private UpgradeField _incomeUpgradesFields;
    public UpgradeField SpeedUpgradeFields { get => _speedUpgradesFields; private set { } }
    public UpgradeField IncomeUpgradesFields { get => _incomeUpgradesFields; private set { } }

    public event Action<int> OnIncomeUpgraded;
    public event Action<int> OnSpeedUpgraded;


    public event Action OnDataInit;

    private void Start()
    {
        capacityUpLevels = _machineBaseFields.GetCapacityLevels();
        UpgradeDataInit();
        _machineBaseFields.SetCapacity(capacityUpLevels[indexer]);

    }
    public void UpgradeSpeedCapacity(int level = 0)
    {

        _machineBaseFields.UpgradeSpeed(_speedUpgradesFields);
        
        CapacityUpgradeCheck();
        OnSpeedUpgraded?.Invoke(_speedUpgradesFields.Level);
        _speedValueText.text = _speedUpgradesFields.FieldValue.ToString("0.##");

        void CapacityUpgradeCheck()
        {
            if (capacityUpLevels[indexer] == _speedUpgradesFields.Level)
            {
                _machineBaseFields.CapacityUp(_machineBaseFields.CapacityDelta);
                _capacityValueText.text = _machineBaseFields.MaxTiles.ToString();
                indexer++;
                UpgradeMesh();
            }
        }
    }

    public void UpgradeIncome(int level = 0)
    {

        _machineBaseFields.UpgradeIncome(_incomeUpgradesFields);
        OnIncomeUpgraded?.Invoke(_incomeUpgradesFields.Level);
        _incomeValueText.text = _incomeUpgradesFields.FieldValue.ToString("0.##");


    }

    private void UpgradeMesh()
    {
        meshFilter.mesh = upgradeMeshes?[indexer - 1];
    }

    protected virtual void UpgradeDataInit()
    {
        if (_speedUpgradesFields.FieldValue != 0)
            return;

        _speedUpgradesFields = new UpgradeField(_machineBaseFields.SpeedData(), SpeedFormula, SpeedPriceFormula);
        _incomeUpgradesFields = new UpgradeField(_machineBaseFields.IncomeData(), IncomeFormula, IncomePriceFormula);

    }

    public MachineUpgradeData GetData()
    {
        MachineUpgradeData data = new();

        data.speedData = _machineBaseFields.SpeedData();
        data.incomeData = _machineBaseFields.IncomeData();
        data.indexer = indexer;

        return data;
    }

    public void Initialize(MachineUpgradeData data)
    {
        indexer = data.indexer;
        if (indexer > 0)
        {
            UpgradeMesh();

        }

        _speedUpgradesFields = new(data.speedData, SpeedFormula, SpeedPriceFormula);
        _incomeUpgradesFields = new(data.incomeData, IncomeFormula, IncomePriceFormula);

        _speedValueText.text = _speedUpgradesFields.FieldValue.ToString("0.##");
        _incomeValueText.text = _incomeUpgradesFields.FieldValue.ToString("0.##");
        _capacityValueText.text = _machineBaseFields.MaxTiles.ToString();

        OnDataInit.Invoke();
    }

    #region Formuly
    float IncomeFormula(float current, float delta)
    {
        float result = current * delta;
        return result;
    }
    float SpeedFormula(float current, float delta)
    {
        float result = current / delta;
        return result;
    }

    float SpeedPriceFormula(float current, float delta)
    {
        float result = current * delta;
        return result;
    }
    float IncomePriceFormula(float current, float delta)
    {
        float result = current * delta;
        return result;
    }

    public void UpgradeIncome()
    {
        throw new NotImplementedException();
    }
    #endregion
}
[Serializable]
public class MachineUpgradeData
{
    public UpgradeNumbersData speedData;
    public UpgradeNumbersData incomeData;

    public int indexer;

    public MachineUpgradeData() 
    {
        speedData = new();
        incomeData = new();
        indexer = 0;
    }
}
public interface IUpgradeMachine
{

    public UpgradeField SpeedUpgradeFields { get; }
    public UpgradeField IncomeUpgradesFields { get; }

    public void UpgradeIncome(int level = 0);
    public void UpgradeSpeedCapacity(int level = 0);
}
