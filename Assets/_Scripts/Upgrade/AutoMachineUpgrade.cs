using System;
using TMPro;
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

    [SerializeField] private TMP_Text _capacityValueText, _speedValueText, _incomeValueText;

    public event Action<int> OnIncomeUpgraded;
    public event Action<int> OnSpeedUpgraded;
    public event Action OnDataInit;
    private void Awake()
    {

        producerFields = GetComponent<TileProducerMachine>().ProducerFields;
    }
    private void Start()
    {


        capacityUpLevels = producerFields.GetCapacityLevels();
        UpgradeDataInit();
        producerFields.SetCapacity(capacityUpLevels[indexer]);
    }

    public void UpgradeSpeedCapacity(int level = 0)
    {

        producerFields.UpgradeSpeed(SpeedUpgradeFields);

        CapacityUpgradeCheck();
        OnSpeedUpgraded?.Invoke(SpeedUpgradeFields.Level);
        _speedValueText.text = SpeedUpgradeFields.FieldValue.ToString("0.##");

        void CapacityUpgradeCheck()
        {
            if (capacityUpLevels?[indexer] == SpeedUpgradeFields.Level)
            {
                producerFields.CapacityUp(producerFields.CapacityDelta);
                _capacityValueText.text = producerFields.MaxTiles.ToString();
                indexer++;
            }
        }
    }

    public void UpgradeIncome(int level = 0)
    {

        producerFields.UpgradeIncome(IncomeUpgradesFields);
        OnIncomeUpgraded?.Invoke(IncomeUpgradesFields.Level);
        _incomeValueText.text = IncomeUpgradesFields.FieldValue.ToString("0.##");

    }


    protected virtual void UpgradeDataInit()
    {
        if (_speedUpgrades.FieldValue != 0)
            return;

        _speedUpgrades = new UpgradeField(producerFields.SpeedData(), SpeedFormula, SpeedPriceFormula);

        _incomeUpgrades = new UpgradeField(producerFields.IncomeData(), IncomeFormula, IncomePriceFormula);



    }

    public MachineUpgradeData GetData()
    {
        MachineUpgradeData _data = new();
        _data.speedData = producerFields.SpeedData();
        _data.incomeData = producerFields.IncomeData();
        _data.indexer = indexer;
        return _data;
    }

    public void Initialize(MachineUpgradeData data)
    {

        _speedUpgrades = new(data.speedData, SpeedFormula, SpeedPriceFormula);
        _incomeUpgrades = new(data.incomeData, IncomeFormula, IncomePriceFormula);
        indexer = data.indexer;

        _speedValueText.text = _speedUpgrades.FieldValue.ToString("0.##");
        _incomeValueText.text = _incomeUpgrades.FieldValue.ToString("0.##");
        _capacityValueText.text = producerFields.MaxTiles.ToString();

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

