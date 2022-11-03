using System.Collections.Generic;
using UnityEngine;

public class RecourceView : MonoBehaviour
{

    [SerializeField] private TileMachine _machine;


    [SerializeField] protected Animator resourceCounterAnimator;
    [SerializeField] protected TMPro.TMP_Text resourceCountText;
    public TileType type;
    private IReadOnlyCollection <Tile> stack;
    private void Start()
    {
        stack = _machine.TilesListsByType[type];
        
    }
    public virtual void ProductTileView(int current)
    {
        resourceCountText.text = $"{stack.Count}";
        resourceCounterAnimator.SetTrigger("Plus");
    }
}




