using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkTilesSpawn : MonoBehaviour
{
    [SerializeField] private Tile junkTile;
    [SerializeField] private int maxJunkTilesAmount;

    private ObjectPooler<Tile> _junkTilesPool;

    private void OnEnable()
    {
        _junkTilesPool = new ObjectPooler<Tile>(junkTile, this.transform);
        _junkTilesPool.CreatePool(maxJunkTilesAmount);
    }
}
