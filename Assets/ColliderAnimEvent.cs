using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAnimEvent : MonoBehaviour
{
    [SerializeField] private BoxCollider _coll;

    public void ColliderTurnOn()
    {
        _coll.enabled = true;
    }
    public void ColliderTurnOff() 
    { 
        _coll.enabled = false;
    }
}
