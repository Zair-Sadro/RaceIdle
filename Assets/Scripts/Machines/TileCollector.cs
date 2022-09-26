using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollector : MonoBehaviour
{
    [SerializeField] protected int maxTileCount;
    [SerializeField] protected int currentTilesCount;

    [SerializeField] protected TMPro.TMP_Text counter;

    protected Action<int> OnCountChange;

    private void OnEnable()
    {
        OnCountChange += TextCountVisual;
    }
    private void OnDisable()
    {
        OnCountChange -= TextCountVisual;
    }
    public virtual void TextCountVisual(int maxValue)
    {
        counter.text = $"{currentTilesCount}/{maxValue}";
    }

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
