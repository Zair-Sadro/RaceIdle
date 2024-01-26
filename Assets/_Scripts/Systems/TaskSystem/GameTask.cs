using UnityEngine;

public class GameTask : MonoBehaviour
{
    public string taskName;
    public float taskReward;
    public Sprite icon;

    [SerializeField] private GameObject objWithTask;
    private IGameTask _igtask;
    public IGameTask Task => _igtask;

    private void Awake()
    {
        _igtask = objWithTask.GetInterface<IGameTask>();
    }
}

