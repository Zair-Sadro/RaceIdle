using UnityEngine;

public class MachineUpgrade : MonoBehaviour
{
    private MachineFields machineFields;

    [HideInInspector]
    public UpgradeField Speed, Income;

    private int[] capacityUpLevels;
    private int indexer;
    
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
            machineFields.Speed = Speed.LevelUp();
            CapacityUpgradeCheck();
        }



         void CapacityUpgradeCheck()
         {
            if (capacityUpLevels[indexer] == Speed.Level)
            {
                machineFields.CapacityUp(machineFields.CapacityDelta);
                indexer++;
            }
         }
    }

    public void UpgradeIncome(int level = 0)
    {
        if (level == 0)
        {
            machineFields.Income = Income.LevelUp();
        }

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

