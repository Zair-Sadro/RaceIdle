using System;
using UnityEngine;

[Serializable]
public class UpgradeNumbersData 
{
    [Tooltip("Начальное значение")]
    public  float startNumber;
    [Tooltip("Коэффициент")]
    public float deltaNumber;
    [Tooltip("Начальное значение цены")]
    public  float startNumberPrice;
    [Tooltip("Коэффициент к цене")]
    public float deltaNumberPrice;

    public float currentValue;
    public float currentPriceValue;

    public int currentLevel;
    public int maxLevel;
    
    public bool IsMaxLevel => currentLevel >= maxLevel;
}

