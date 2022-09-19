using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollector : MonoBehaviour
{
    [SerializeField] private int maxTileCount;
    [SerializeField] private int currentTilesCount;
    public virtual void Collect() { }
    public virtual void Remove() { }
}
public interface IProduce
{
    [Tooltip("Tilles needed for 1 product")]
    public int tilesNeeded { get; set; }
    public int productMaxCount { get; set; }
    public void Produce(Action action);
}
