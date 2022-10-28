using System;
using UnityEngine;

[Serializable]
public class MachineNumbersData 
{
    public  float startNumber;
    [SerializeField] private float deltaNumber;
    public  float startNumberPrice;
    [SerializeField] private float deltaNumberPrice;

    public float DeltaNumber { get => deltaNumber; private set => deltaNumber = value; }
    public float DeltaNumberPrice { get => deltaNumberPrice; private set => deltaNumberPrice = value; }
}

