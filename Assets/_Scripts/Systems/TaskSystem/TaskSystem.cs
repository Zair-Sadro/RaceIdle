using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text reward;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject taskPanel;

    [SerializeField] private WalletSystem wallet;

    [SerializeField] private List<GameTask> tasks;

    private int _currentTaskIndx;
    private GameTask _currentTask;
    private float _currentReward;

    public int CurrentTaskIndx => _currentTaskIndx;
    private void Start()
    {
        SetTask(0);
    }
    public void SetTaskFromSave(int i)
    {
        SetTask(i);
    }
    private void SetTask(int indx)
    {

        if (indx == -1 || indx == tasks.Count)
        {
            _currentTaskIndx = -1;
            taskPanel.SetActive(false);
            return;
        }

        _currentTask = tasks[indx];

        TaskView();

        _currentTask.Task.TaskDone += OnTaskDone;
        _currentTask.Task.StartTask();

        _currentReward = _currentTask.taskReward;
        _currentTaskIndx = indx;

        void TaskView()
        {
            if (_currentTask.icon != null)
                icon.sprite = _currentTask.icon;

            text.text = _currentTask.taskName;
            reward.text = _currentTask.taskReward.ToString();
        }
    }

    private void OnTaskDone()
    {
        _currentTask.Task.TaskDone -= OnTaskDone;
        wallet.Income(_currentReward);

        SetTask(++_currentTaskIndx);
    }

}
public interface IGameTask
{
    public event Action TaskDone;
    public void StartTask();
}
public class CarMergeTask : MonoBehaviour, IGameTask
{
    public event Action TaskDone;
    public void StartTask()
    {
        throw new NotImplementedException();
    }
}

