using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MachineUI : MonoBehaviour
{
    [SerializeField] private TileMachine _machine;
    [SerializeField] private List<TileDisplayer> _tileDisplayers;



    private void Start()
    {
        Init();
    }
    private void Init()
    {
      
        
    }

    public void ChangeCount()
    {

    }
}

[Serializable]
public sealed class TileDisplayer
{
    public int currentCount;
    public TileType type;

    public TileDisplayer (ProductRequierment product)
    {
        type = product.Type;
        currentCount = 0;

    }
}
