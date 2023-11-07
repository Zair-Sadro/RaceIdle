using System;
using UnityEngine;

public class AutoMachineUpgrade : MonoBehaviour, ISaveLoad<MachineUpgradeData>, IUpgradeMachine
{

    private ProducerFields producerFields;
    private int[] capacityUpLevels;
    private int indexer;

    private UpgradeField _speedUpgrades;
    private UpgradeField _incomeUpgrades;
    public UpgradeField SpeedUpgradeFields { get => _speedUpgrades; private set { } }
    public UpgradeField IncomeUpgradesFields { get => _incomeUpgrades; private set { } }

    public event Action<int> OnIncomeUpgraded;

    private void Awake()
    {
        producerFields = GetComponent<TileProducerMachine>().ProducerFields;
        capacityUpLevels = producerFields.GetCapacityLevels();
        UpgradeDataInit();

    }
    private void Start()
    {
         
        producerFields.SetCapacity(capacityUpLevels[indexer]);
    }

    public void UpgradeSpeedCapacity(int level = 0)
    {
        if (level == 0)
        {
            producerFields.UpgradeSpeed(SpeedUpgradeFields);
            CapacityUpgradeCheck();
        }


        void CapacityUpgradeCheck()
        {
            if (capacityUpLevels?[indexer] == SpeedUpgradeFields.Level)
            {
                producerFields.CapacityUp(producerFields.CapacityDelta);
                indexer++;
            }
        }
    }

    public void UpgradeIncome(int level = 0)
    {
        if (level == 0)
        {
            producerFields.UpgradeIncome(IncomeUpgradesFields);
            OnIncomeUpgraded.Invoke(IncomeUpgradesFields.Level);
        }

    }


    protected virtual void UpgradeDataInit()
    {
        if (_speedUpgrades != null)
            return;
        _speedUpgrades = new UpgradeField(producerFields.SpeedData(), SpeedFormula, SpeedPriceFormula);
        _incomeUpgrades = new UpgradeField(producerFields.IncomeData(), IncomeFormula, IncomePriceFormula);



    }

    public MachineUpgradeData GetData()
    {
        MachineUpgradeData _data=new();
        _data.speedData = producerFields.SpeedData();
        _data.incomeData = producerFields.IncomeData();
        _data.indexer = indexer;
        return _data;
    }

    public void Initialize(MachineUpgradeData data)
    {

        SpeedUpgradeFields = new(data.speedData, SpeedFormula, SpeedPriceFormula);
        IncomeUpgradesFields = new(data.incomeData, IncomeFormula, IncomePriceFormula);
        indexer = data.indexer;

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

