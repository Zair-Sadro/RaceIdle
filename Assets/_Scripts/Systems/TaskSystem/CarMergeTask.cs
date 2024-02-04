using System;
using UnityEngine;

public class CarMergeTask : MonoBehaviour, IGameTask
{
    [SerializeField] private MergeMaster mergeMaster;
    public event Action TaskDone;

    public void EndTask()
    {
        mergeMaster.CarMergedEvent -= TaskDone;
    }

    public void StartTask()
    {
        mergeMaster.CarMergedEvent += TaskDone;
    }
}


