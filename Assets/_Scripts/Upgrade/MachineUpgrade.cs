using UnityEngine;

public class MachineUpgrade : MonoBehaviour
{
    [SerializeField] private Mesh[] upgradeMeshes;
    [SerializeField] private MeshFilter meshFilter;

    [HideInInspector]
    public UpgradeField Speed, Income;
    private MachineFields machineFields;

    private int[] capacityUpLevels;
    private int indexer;

    public event System.Action<int> OnIncomeUpgraded;

    

    private void Awake()
    {
        machineFields = GetComponent<TileMachine>().machineFields;
        capacityUpLevels = machineFields.GetCapacityLevels();


        UpgradeDataInit();
    }
    public void UpgradeSpeedCapacity(int level = 0)
    {
        if (level == 0)
        {
            machineFields.UpgradeSpeed(Speed);
            CapacityUpgradeCheck();
        }



         void CapacityUpgradeCheck()
         {
            if (capacityUpLevels?[indexer] == Speed.Level)
            {
                machineFields.CapacityUp(machineFields.CapacityDelta);
                indexer++;
                UpgradeMesh();
            }
         }
    }

    public void UpgradeIncome(int level = 0)
    {
        if (level == 0)
        {
            machineFields.UpgradeIncome(Income);
            OnIncomeUpgraded.Invoke(Income.Level);
        }

    }

    private void UpgradeMesh()
    {
        meshFilter.mesh = upgradeMeshes?[indexer-1];
    }

    protected virtual void UpgradeDataInit()
    {
        Speed =  new UpgradeField(machineFields.SpeedData(),  SpeedFormula,SpeedPriceFormula);
        Income = new UpgradeField(machineFields.IncomeData(), IncomeFormula,IncomePriceFormula);       

    }
    float IncomeFormula(float current, float delta)
    {
        float result = current * delta;
        return result;
    }
    float SpeedFormula(float current,float delta)
    {
        float result = current / delta;
        return result;
    }

    float SpeedPriceFormula(float current, float delta)
    {
        float result = current * delta;
        return result;
    }
    float IncomePriceFormula(float current,float delta)
    {
        float result = current * delta;
        return result;
    }
   
    
}

