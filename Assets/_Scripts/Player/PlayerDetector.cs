using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerDetector : MonoBehaviour
{
    internal event Action OnPlayerEnter, OnPlayerExit, OnPlayerStay;

    protected virtual void OnTriggerEnter(Collider other)
    {
       if (OnPlayerEnter != null)
        if (IsPlayer(other.gameObject))
        {
            OnPlayerEnter.Invoke();
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
       if (OnPlayerExit != null)
        if (IsPlayer(other.gameObject))
        {
            OnPlayerExit.Invoke();
        }
    }
    protected virtual void OnTriggerStay(Collider other)
    {
       if (OnPlayerStay!=null)
        if (IsPlayer(other.gameObject))
        {
            OnPlayerStay.Invoke();
        }
    }


    public static bool IsPlayer(GameObject ob)
    {
        if (ob.CompareTag("Player"))
            return true;

        return false;
    }
}

public class SlashDetector : PlayerDetector
{ 
    [SerializeField] private string gameObjTag;

    private void Awake()
    {
        
    }
    protected override void OnTriggerEnter(Collider other)
    {
      base.OnTriggerEnter(other);

    }
    protected override void OnTriggerExit(Collider other)
    {
      base.OnTriggerExit(other);

    }
    protected override void OnTriggerStay(Collider other)
    {
     base.OnTriggerStay(other);

    }

}

public interface IActionInZone
{
    public void DoZoneAction();
}

