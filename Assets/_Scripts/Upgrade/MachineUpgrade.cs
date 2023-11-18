using System;
using UnityEngine;

public class MachineUpgrade : MonoBehaviour, ISaveLoad<MachineUpgradeData>, IUpgradeMachine
{
    [SerializeField] private Mesh[] upgradeMeshes;
    [SerializeField] private MeshFilter meshFilter;

    [SerializeField] private MachineFields machineFields;

    private int[] capacityUpLevels;
    private int indexer;
    private int maxLevelIndex;

    private UpgradeField _speedUpgradesFields;
    private UpgradeField _incomeUpgradesFields;
    public UpgradeField SpeedUpgradeFields { get => _speedUpgradesFields; private set { } }
    public UpgradeField IncomeUpgradesFields { get => _incomeUpgradesFields; private set { } }

    public event Action<int> OnIncomeUpgraded;

    private void Start()
    {
        capacityUpLevels = machineFields.GetCapacityLevels();
        maxLevelIndex = capacityUpLevels.Length - 1;
        UpgradeDataInit();
        machineFields.SetCapacity(capacityUpLevels[indexer]);

    }
    public void UpgradeSpeedCapacity(int level = 0)
    {

        machineFields.UpgradeSpeed(_speedUpgradesFields);
        CapacityUpgradeCheck();

        void CapacityUpgradeCheck()
        {
            if (capacityUpLevels[indexer] == _speedUpgradesFields.Level)
            {
                machineFields.CapacityUp(machineFields.CapacityDelta);
                indexer++;
                UpgradeMesh();
            }
        }
    }

    public void UpgradeIncome(int level = 0)
    {

        machineFields.UpgradeIncome(_incomeUpgradesFields);
        OnIncomeUpgraded.Invoke(_incomeUpgradesFields.Level);


    }

    private void UpgradeMesh()
    {
        meshFilter.mesh = upgradeMeshes?[indexer - 1];
    }

    protected virtual void UpgradeDataInit()
    {
        if (_speedUpgradesFields != null)
            return;

        _speedUpgradesFields = new UpgradeField(machineFields.SpeedData(), SpeedFormula, SpeedPriceFormula);
        _incomeUpgradesFields = new UpgradeField(machineFields.IncomeData(), IncomeFormula, IncomePriceFormula);

    }

    public MachineUpgradeData GetData()
    {
        MachineUpgradeData data = new();

        data.speedData = machineFields.SpeedData();
        data.incomeData = machineFields.IncomeData();
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
    public MachineNumbersData speedData;
    public MachineNumbersData incomeData;

    public int indexer;
}
public interface IUpgradeMachine
{

    public UpgradeField SpeedUpgradeFields { get; }
    public UpgradeField IncomeUpgradesFields { get; }

    public void UpgradeIncome(int level = 0);
    public void UpgradeSpeedCapacity(int level = 0);
}
