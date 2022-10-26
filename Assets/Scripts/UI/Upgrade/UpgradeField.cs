using System;

[System.Serializable]
public class UpgradeField
{
    private float startField;
    private float currentField;
    private Func<float,int,float> formula;

    private UpgradePrice Price;

    public float CurrentField => currentField != 0 ? currentField  : startField;
    public (float,float) LevelUp(int level)
    {
        Price.LevelUp(level);
        currentField = formula(startField,level);
        var d = LevelUp(1);
        return (currentField, Price.LevelUp(level));
    }
    public UpgradeField(MachineLevel machineData, Func<float, int, float> formula)
    {
        this.startField = machineData.CreateTime;
        this.formula = formula;
        Price = new UpgradePrice(machineData.startPrice, machineData.PriceFormula);
    }
}
[Serializable]
public class UpgradePrice
{
    private float startPrice;
    private float currentPrice;
    private Func<float, int, float> priceFormula;

    public float CurrentPrice => currentPrice != 0 ? currentPrice : startPrice;
    public float LevelUp(int level)
    {
        currentPrice = priceFormula(startPrice, level);
        return currentPrice;
    }

    public UpgradePrice(float startPrice, Func<float, int, float> priceFormula)
    {
        this.startPrice = startPrice;
        this.priceFormula = priceFormula;
    }
}


