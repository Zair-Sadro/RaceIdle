﻿using System;
using UnityEngine;

public class BuildTask : MonoBehaviour, IGameTask
{

    [SerializeField] private BuilderFromTiles builderFromTiles;
    public event Action TaskDone;
    public void StartTask()
    {
        if (builderFromTiles.IsBuilt)
        {
            TaskDone.Invoke();
            return;
        }
        builderFromTiles.OnBuild += TaskDone.Invoke;
    }
}

