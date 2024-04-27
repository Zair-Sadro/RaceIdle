using UnityEngine;

public class TileCollectorLock : MonoBehaviour, IUnlockAfterTask
{
    [SerializeField] private Collider _collectCollider;
    [SerializeField] private GameObject _locker;

    private bool _unlocked;
    private void Start()
    {
        if (_unlocked)
            return;

        _locker.gameObject.SetActive(true);
        _collectCollider.enabled = false;
    }
    public void Unlock()
    {
        _unlocked = true;

        _locker.gameObject.SetActive(false);
        _collectCollider.enabled = true;
    }

}


