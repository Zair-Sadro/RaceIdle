using UnityEngine;
using YG;

public class GameTask : MonoBehaviour
{
    public string taskName;
    public string taskNameRU;
    public float taskReward;
    public Sprite icon;

    [SerializeField] private GameObject objWithTask;
    private IGameTask _igtask;
    public IGameTask Task => _igtask;

    public string TaskName
    {
        get
        {
            if (l == "ru")
                return taskNameRU;
            else
                return taskName;
        }
    }




    private void Awake()
    {

        YandexGame.GetDataEvent += CheckTranslate;

        _igtask = objWithTask.GetInterface<IGameTask>();
    }
    private string l;
    private void CheckTranslate()
    {
        l = YandexGame.EnvironmentData.language;
    }
}

