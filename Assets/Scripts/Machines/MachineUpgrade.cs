using UnityEngine;

public class MachineUpgrade : MonoBehaviour
{
    private MachineFields machineFields;

    [HideInInspector]
    public UpgradeField Speed, Income;

    private int[] capacityUpLevels;
    
    private void Awake()
    {
        machineFields = GetComponent<TileMachine>().machineFields;
        capacityUpLevels = machineFields.GetCapacityLevels();
    }
    public void UpgradeSpeedCapacity(int level = 0)
    {
        if (level == 0)
        {
            Speed.LevelUp();
        }
         
    }

    public void UpgradeIncome(int level = 0)
    {
        if (level == 0)
        {
            Income.LevelUp();
        }

    }


    protected virtual void UpgradeDataInit()
    {
        Speed = new UpgradeField(machineFields.SpeedData(), SpeedFormula);
        Income = new UpgradeField(machineFields.IncomeData(), IncomeFormula);

    }
    float IncomeFormula(int level)
    {
        return 0f;
    }
    float SpeedFormula(int level)
    {
        return 0f;
    }
   
    
}

