using System;

[System.Serializable]
public class UpgradeField
{
    private float startField;
    private int level;
    private int maxLevel;
    private float currentField;
    private float delta;
    private Func<float, float, float> formula;

    private UpgradePrice Price;
    public float CurrentPrice => Price.CurrentPrice;

    public float FieldValue => currentField != 0 ? currentField : startField;
    public float StartField => startField;
    public int Level => level;

    public bool MaxLevel => maxLevel == level;

    public float LevelUp()
    {
        level++;
        Price.PriceUp(level);
        currentField = formula(FieldValue, delta);

        return currentField;

    }
    public UpgradeValues GetValues()
    {
        UpgradeValues values = new UpgradeValues
            (Price.NextPrice(), FieldValue);

        return values;
    }


    public UpgradeField(UpgradeNumbersData machineData,
        Func<float, float, float> formula, Func<float, float, float> priceFormula)
    {
        this.currentField = machineData.currentValue;
        this.startField = machineData.startNumber;
        this.delta = machineData.DeltaNumber;
        this.formula = formula;
        this.maxLevel = machineData.maxLevel;
        level = machineData.currentLevel;

        Price = new UpgradePrice(machineData.currentPriceValue, machineData.startNumberPrice, machineData.DeltaNumberPrice, priceFormula);
    }
}
[Serializable]
public class UpgradePrice
{
    private float startPrice;
    private float deltaPrice;
    private float currentPrice;

    private Func<float, float, float> priceFormula;
    public float CurrentPrice => currentPrice != 0 ? currentPrice : startPrice;
    public float StartPrice => startPrice;
    public float PriceUp(int level)
    {
        currentPrice = priceFormula(CurrentPrice, deltaPrice);
        return currentPrice;
    }
    public float NextPrice()
    {
        return priceFormula(CurrentPrice, deltaPrice);
    }
    public UpgradePrice(float currentPriceValue, float startPrice, float deltaPrice, Func<float, float, float> priceFormula)
    {
        currentPrice = currentPriceValue;
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



