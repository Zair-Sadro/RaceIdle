using UnityEngine;

[System.Serializable]
public class ProductRequierment
{
    [SerializeField] private TileType _type;
    [SerializeField] private int _amount = 0;

    public TileType Type => _type;
    public int Amount => _amount;
    public ProductRequierment(TileType type, int count)
    {
        _type = type;
        _amount = count;
    }
}

