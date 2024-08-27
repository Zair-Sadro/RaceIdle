using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class TaskSystem : MonoBehaviour
{
    [SerializeField] private Button claimButton;

    [SerializeField] private GameObject taskPanel;
    [SerializeField] private Transform _tutorialArrow;

    [SerializeField] private TaskView _taskView;
    [SerializeField] private WalletSystem wallet;
    [SerializeField] private List<GameTask> tasks;

    private int _currentTaskIndx;
    private GameTask _currentTask;
    private float _currentReward;
    private GameObject _taskUiArrow;

    public int CurrentTaskIndx => _currentTaskIndx;
    private void Awake()
    {
        YandexGame.GetDataEvent += () => _taskView.RewriteTaskText(_currentTask.TaskName);
        claimButton.onClick.AddListener(ClaimReward);
    }

    public void SetTaskFromSave(int i)
    {
        UnlockPastTasksLocks(i);

        _currentTaskIndx = i;
        SetTask(i);

        void UnlockPastTasksLocks(int taskIndx)
        {
            for (int j = 0; j < taskIndx; j++)
            {
                tasks[j].UnlockAfterTask?.Unlock();
            }
        }
    }



    private void ClaimReward()
    {
        wallet.Income(_currentReward);
        _taskView.OnClaimReward();

        SetTask(_currentTaskIndx);
    }


    private void SetTask(int indx)
    {

        if (indx == -1 || indx == tasks.Count)
        {
            OnTaskFinished();
            return;
        }

        _currentTask = tasks[indx];

        _taskView.SetTaskTextData(_currentTask);
        SetArrow();

        _currentTask.Task.TaskDone += OnTaskDone;
        _currentReward = _currentTask.taskReward;
        _currentTaskIndx = indx;

        _currentTask.Task.StartTask();

        void SetArrow()
        {
            var pos = _currentTask.taskPosForArrow;
            _taskUiArrow = _currentTask.arrowInUI;

            if (pos != null)
            {
                _tutorialArrow.gameObject.SetActive(true);
                _tutorialArrow.transform.position = pos.transform.position;
            }
               
            
            if (_taskUiArrow != null)
                _taskUiArrow.SetActive(true);
        }

    }

    private void OnTaskFinished()
    {

        DisableArrows();
        
        _currentTaskIndx = -1;
        taskPanel.SetActive(false);
        
    }

    private void OnTaskDone()
    {
        DisableArrows();
        
        _currentTask.UnlockAfterTask?.Unlock();
        _currentTask.Task.EndTask();
        _currentTask.Task.TaskDone -= OnTaskDone;
        ++_currentTaskIndx;

        _taskView.OnTaskDone();

    }
   private  void DisableArrows()
    {
        if (_taskUiArrow != null)
            _taskUiArrow.SetActive(false);

        _taskUiArrow = null;
        _tutorialArrow.gameObject.SetActive(false);
    }

}
public interface IGameTask
{
    public event Action TaskDone;
    public void StartTask();
    public void EndTask();
}
public interface IUnlockAfterTask
{
    public void Unlock();
}


