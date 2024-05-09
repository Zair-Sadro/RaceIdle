using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CarMergeTask : MonoBehaviour, IGameTask
{
    [SerializeField] private MergeMaster mergeMaster;
    [SerializeField] private Image _mergeButtImage;
    [SerializeField] private Button _mergeButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private CanvasGroup _tutorialTextObj;
    public event Action TaskDone;

    public void EndTask()
    {
        mergeMaster.CarMergedEvent -= TaskDone;

        _tutorialTextObj.DOFade(0,0.7f);
        _mergeButtImage.DORewind();

        _mergeButton.onClick.RemoveListener(ShowTutorial);
        _backButton.onClick.RemoveListener(HideTutorial);
    }

    public void StartTask()
    {
        
        mergeMaster.CarMergedEvent += TaskDone;
        _mergeButtImage.DOColor(Color.red, 1f).SetLoops(-1,LoopType.Yoyo);

        _mergeButton.onClick.AddListener(ShowTutorial);
        _backButton.onClick.AddListener(HideTutorial);
    }
    private void ShowTutorial()
    {
        _mergeButtImage.DORewind();
        _tutorialTextObj.DOFade(1, 0.7f);
    }
    private void HideTutorial() 
    {
        _tutorialTextObj.DOFade(0, 0.7f);
    }
}


