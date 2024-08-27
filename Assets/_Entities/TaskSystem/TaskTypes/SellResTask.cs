using System;
using UnityEngine;

public class SellResTask : MonoBehaviour, IGameTask
{
    
    public event Action TaskDone;
    public void StartTask()
    {
        GameEventSystem.TileSold += OnTileSold;
    }

    public void EndTask()
    {
        GameEventSystem.TileSold -= OnTileSold;
    }

    private void OnTileSold(TileType t)
    {
        TaskDone?.Invoke();
    }
}
