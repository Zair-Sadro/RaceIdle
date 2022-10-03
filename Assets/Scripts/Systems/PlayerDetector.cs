using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Action OnPlayerEnter, OnPlayerExit, OnPlayerStay;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            OnPlayerEnter?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            OnPlayerExit?.Invoke();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            OnPlayerStay?.Invoke();
        }
    }


    public static bool IsPlayer(GameObject ob)
    {
        if (ob.CompareTag("Player"))
            return true;

        return false;
    }
}

