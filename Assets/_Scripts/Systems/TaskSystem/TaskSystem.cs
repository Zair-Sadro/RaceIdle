using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text reward;
    [SerializeField] private CanvasGroup OnDonePanel;
    [SerializeField] private Image buttonGraphic;
    [SerializeField] private Button claimButton;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject taskPanel;

    [SerializeField] private WalletSystem wallet;
    [SerializeField] private List<GameTask> tasks;

    private int _currentTaskIndx;
    private GameTask _currentTask;
    private float _currentReward;

    public int CurrentTaskIndx => _currentTaskIndx;
    private void Awake()
    {
        claimButton.onClick.AddListener(ClaimReward);
    }

    private void ClaimReward()
    {
        wallet.Income(_currentReward);
        OnDonePanel.blocksRaycasts = false;
        OnDonePanel.DOFade(0, 0.4f);
        buttonGraphic.DORewind();
        buttonGraphic.DOKill();

        SetTask(_currentTaskIndx);
    }

    public void SetTaskFromSave(int i)
    {
        _currentTaskIndx = i;
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
        _currentReward = _currentTask.taskReward;
        _currentTaskIndx = indx;

        _currentTask.Task.StartTask();



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
        _currentTask.Task.EndTask();
        _currentTask.Task.TaskDone -= OnTaskDone;
        ++_currentTaskIndx;

        OnDonePanel.blocksRaycasts = true;
        OnDonePanel.DOFade(1, 0.4f);
        buttonGraphic.DOColor(Color.green, 0.6f).SetLoops(-1, LoopType.Yoyo);
    }

}
public interface IGameTask
{
    public event Action TaskDone;
    public void StartTask();
    public void EndTask();
}
public class CarMergeTask : MonoBehaviour, IGameTask
{
    public event Action TaskDone;

    public void EndTask()
    {
        throw new NotImplementedException();
    }

    public void StartTask()
    {
        throw new NotImplementedException();
    }
}

public class CarSpawnTask : MonoBehaviour, IGameTask
{
    [SerializeField] private CarSpawner spawner;
    public event Action TaskDone;

    public void EndTask()
    {
        spawner.OnCarSpawned -= (t) => TaskDone();
    }

    public void StartTask()
    {
        spawner.OnCarSpawned += (t) => TaskDone();
    }
}

