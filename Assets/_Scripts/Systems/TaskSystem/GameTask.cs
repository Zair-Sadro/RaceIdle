using UnityEngine;
using YG;

public class GameTask : MonoBehaviour
{
    public string taskName;
    public string taskNameRU;
    public float taskReward;

    public Transform taskPosForArrow;
    public GameObject arrowInUI;
    public Sprite icon;
    
    private IGameTask _igtask;
    private IUnlockAfterTask _unlockAfterTask;
    public IGameTask Task => _igtask;
    public IUnlockAfterTask UnlockAfterTask => _unlockAfterTask;

    public string TaskName
    {
        get { return l == "ru" ? taskNameRU : taskName; }
    }

    private void Awake()
    {

        YandexGame.GetDataEvent += CheckTranslate;

        _igtask = gameObject.GetInterface<IGameTask>();
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

