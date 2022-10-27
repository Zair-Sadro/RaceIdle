using System;

[System.Serializable]
public class UpgradeField
{
    private float startField;
    private int level;
    private float currentField;
    private Func<int, float> formula;

    private UpgradePrice Price;

    public float FieldValue => currentField != 0 ? currentField  : startField;
    public int Level => level;

    public void LevelUp()
    {
        ++level;
        Price.NextPrice(level);
        currentField = formula(level);
       
    }
    public UpgradeValues GetValues()
    {
        UpgradeValues values = new UpgradeValues
            (Price.NextPrice(level) , currentField);

        return values;
    }
   
    public UpgradeField(MachineNumbersData machineData, Func<int, float> formula)
    {
        this.startField = machineData.startNumber;
        this.formula = formula;
        Price = new UpgradePrice(machineData.startNumberPrice, machineData.DeltaNumberPrice);
    }
}
[Serializable]
public class UpgradePrice
{
    private float startPrice;
    private float deltaPrice;
    private float currentPrice;

    private Func<int, float> priceFormula;
    public float CurrentPrice => currentPrice != 0 ? currentPrice : startPrice;
    public float NextPrice(int level)
    {
        currentPrice = priceFormula(++level);
        return currentPrice;
    }

    public UpgradePrice(float startPrice, float deltaPrice)
    {
        this.startPrice = startPrice;
        this.deltaPrice = deltaPrice;

        this.priceFormula = DefaultFormula;

    }
    public UpgradePrice(float startPrice, float deltaPrice, Func<int, float> priceFormula)
    {
        this.startPrice = startPrice;
        this.deltaPrice = deltaPrice;
        this.priceFormula = priceFormula;
    }

    private float DefaultFormula(int level)
    {
        float result;

        result = currentPrice * level * deltaPrice;

        return result;
    }
}
public class UpgradeValues
{
    public float price;
    public float value;

    public UpgradeValues(float price, float value)
    {
        this.price = price;
        this.value = value;
    }
}


