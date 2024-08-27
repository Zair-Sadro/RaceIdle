using UnityEngine;

public class TileCollectorLock : MonoBehaviour, IUnlockAfterTask
{
    [SerializeField] private Collider _collectCollider;
    [SerializeField] private GameObject _locker;
    [SerializeField] private GameObject[] _countersElements;
    [SerializeField] private string counterName;
    

    private bool _unlocked;
    private void Start()
    {
        if (_unlocked)
            return;

        _locker.gameObject.SetActive(true);
        _collectCollider.enabled = false;
        
        for (int i = 0; i < _countersElements.Length; i++)
            _countersElements[i].SetActive(false);
    }
    public void Unlock()
    {
        _unlocked = true;

        _locker.gameObject.SetActive(false);
        _collectCollider.enabled = true;
        
        for (int i = 0; i < _countersElements.Length; i++)
            _countersElements[i].SetActive(true);
    }

}


