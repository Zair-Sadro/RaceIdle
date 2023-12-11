using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerDetector : MonoBehaviour
{
    internal event Action OnPlayerEnter, OnPlayerExit, OnPlayerStay;
    [SerializeField] private bool needTimerForNextInvoke;
    private bool isTimer;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (isTimer)
            return;

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

        if (needTimerForNextInvoke)
            StartCoroutine(TimerForNext());
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (isTimer)
            return;

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
    IEnumerator TimerForNext() 
    {
        isTimer = true;
        yield return new WaitForSeconds(1f);
        isTimer = false;
    }
}


