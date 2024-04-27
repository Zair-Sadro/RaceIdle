using UnityEngine;
using YG;

public class GameTask : MonoBehaviour
{
    public string taskName;
    public string taskNameRU;
    public float taskReward;

    public Transform taskPosForArrow;
    public Sprite icon;

    [SerializeField] private GameObject objWithTask;
    private IGameTask _igtask;
    private IUnlockAfterTask _unlockAfterTask;
    public IGameTask Task => _igtask;
    public IUnlockAfterTask UnlockAfterTask => _unlockAfterTask;

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
        _unlockAfterTask = gameObject.GetInterface<IUnlockAfterTask>(false);
    }
    private string l;
    private void CheckTranslate()
    {
        //#if UNITY_EDITOR
        //        l = YandexGame.savesData.language;

        //#else
        //                l = YandexGame.EnvironmentData.language;
        //#endif

        l = YandexGame.EnvironmentData.language;


    }
}

