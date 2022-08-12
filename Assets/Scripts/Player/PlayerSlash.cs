using System;
using System.Collections;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    [SerializeField] private float activeHammerCollTime;
    [SerializeField] private PlayerController controller;
    [SerializeField] private Collider hammerColl;

    private bool _canSlash = true;

    private void Slash()
    {
        if(_canSlash)
            StartCoroutine(SlashRoutine(activeHammerCollTime));
    }

    private IEnumerator SlashRoutine(float time)
    {
        _canSlash = false;
        controller.Animator.SetTrigger("Slash");
        hammerColl.enabled = true;

        yield return new WaitForSeconds(time);
        hammerColl.enabled = false;
        _canSlash = true;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out JunkCar junkcar))
        {
            if(junkcar.CanBeDamaged)
                Slash();
        }
    }

}
