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

    public List<Tile> GetRandomJunkTiles(int min, int max)
    {
        List<Tile> newList = new List<Tile>();
        int randomAmount = Random.Range(min, max);

        for (int i = 0; i < randomAmount; i++)
            newList.Add(_junkTilesPool.GetFreeObject());

        return newList;
    }
}
