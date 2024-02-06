using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAnimEvent : MonoBehaviour
{
    [SerializeField] private BoxCollider _coll;
    [SerializeField] private GameObject _boomEfect;

    public void ColliderTurnOn()
    {
        _coll.enabled = true;
        _boomEfect.SetActive(true);

    }
    public void ColliderTurnOff() 
    { 
        _coll.enabled = false;
    }
}
