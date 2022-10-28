using System;

[System.Serializable]
public class UpgradeField
{
    private float startField;
    private int level;
    private float currentField;
    private float delta;
    private Func<float,float, float> formula;

    private UpgradePrice Price;

    public float FieldValue => currentField != 0 ? currentField : startField;
    public float StartField => startField;
    public int Level => level;

    public float LevelUp()
    {
        ++level;
        Price.NextPrice(level);
        currentField = formula(currentField,delta);
  
        return currentField;
       
    }
    public UpgradeValues GetValues()
    {
        UpgradeValues values = new UpgradeValues
            (Price.NextPrice(level) , currentField);

        return values;
    }
    
   
    public UpgradeField(MachineNumbersData machineData, Func<float, float, float> formula, Func<float,float,float> priceFormula)
    {
        this.startField = machineData.startNumber;
        this.formula = formula;
        Price = new UpgradePrice(machineData.startNumberPrice, machineData.DeltaNumberPrice,priceFormula);
    }
}
[Serializable]
public class UpgradePrice
{
    private float startPrice;
    private float deltaPrice;
    private float currentPrice;

    private Func<float,float, float> priceFormula;
    public float CurrentPrice => currentPrice != 0 ? currentPrice : startPrice;
    public float StartPrice => startPrice;
    public float NextPrice(int level)
    {
        currentPrice = priceFormula(currentPrice, deltaPrice);
        return currentPrice;
    }
    public UpgradePrice(float startPrice, float deltaPrice, Func<float,float, float> priceFormula)
    {
        this.startPrice = startPrice;
        this.deltaPrice = deltaPrice;
        this.priceFormula = priceFormula;
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



