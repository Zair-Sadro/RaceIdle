﻿using System;
using UnityEngine;

[Serializable]
public class MachineNumbersData 
{
    [Tooltip("Начальное значение")]
    public  float startNumber;
    [Tooltip("Коэффициент")]
    [SerializeField] private float deltaNumber;
    [Tooltip("Начальное значение цены")]
    public  float startNumberPrice;
    [Tooltip("Коэффициент к цене")]
    [SerializeField] private float deltaNumberPrice;

    public float currentValue;
    public float currentPriceValue;

    public float DeltaNumber { get => deltaNumber; private set => deltaNumber = value; }
    public float DeltaNumberPrice { get => deltaNumberPrice; private set => deltaNumberPrice = value; }
}

