using System;
using UnityEngine;

    public class DestroyCarTask : MonoBehaviour, IGameTask
    {
        [SerializeField] private JunkCarManager cars;
        public event Action TaskDone;
        public void StartTask()
        {
            cars.OnAnyCarDestroy += TaskDone.Invoke;
        }
        
        public void EndTask()
        {
            cars.OnAnyCarDestroy -= TaskDone.Invoke;
        }
    }