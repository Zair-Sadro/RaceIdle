using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollector : MonoBehaviour
{

    [SerializeField] protected Animator counterAnimator;

    [SerializeField] protected TMPro.TMP_Text counter;

    protected Action<int> OnCountChange;


    [SerializeField] protected virtual int maxTileCount { get; private set; }
    [SerializeField] protected virtual int currentTilesCount { get; set; }

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
        counterAnimator.SetTrigger("Plus");
    }

    public virtual void Collect() { }
    public virtual void Remove() { }
}
public interface IProduce
{

    public void Produce();
}
