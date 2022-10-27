using System;

[Serializable]
public class MachineNumbersData 
{
    public  float startNumber;
    private float deltaNumber;
    public  float startNumberPrice;
    private float deltaNumberPrice;

    public float DeltaNumber { get => deltaNumber; private set => deltaNumber = value; }
    public float DeltaNumberPrice { get => deltaNumberPrice; private set => deltaNumberPrice = value; }
}

