using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskView : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text reward;
    [SerializeField] private CanvasGroup OnDonePanel;

    [SerializeField] private Image buttonGraphic;
    [SerializeField] private Image icon;

    public void RewriteTaskText(string taskName)
    {
        text.text = taskName;
    }

    public void OnClaimReward()
    {
        OnDonePanel.blocksRaycasts = false;
        OnDonePanel.DOFade(0, 0.4f);
        buttonGraphic.DORewind();
        buttonGraphic.DOKill();
    }
    public void OnTaskDone()
    {

        OnDonePanel.blocksRaycasts = true;
        OnDonePanel.DOFade(1, 0.4f);
        buttonGraphic.DOColor(Color.green, 0.6f).SetLoops(-1, LoopType.Yoyo);
    }
    public void SetTaskTextData(GameTask task)
    {
        if (task.icon != null)
            icon.sprite = task.icon;

        text.text = task.TaskName;
        reward.text = task.taskReward.ToString();
    }

}


