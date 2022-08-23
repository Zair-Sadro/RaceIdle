using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerSlash : MonoBehaviour
{
    [SerializeField] private float activeHammerCollTime;
    [SerializeField] private float delayBeforeDamage;

    [Inject] private PlayerController controller;

    private bool _canSlash = true;
    private void Slash(JunkCar car)
    {
        if (_canSlash)
        {
            StartCoroutine(SlashRoutine(activeHammerCollTime));
            car.TakeDamage(delayBeforeDamage);
        }
           
    }

    private IEnumerator SlashRoutine(float time)
    {
        _canSlash = false;
        controller.Animator.SetTrigger("Slash");

        yield return new WaitForSeconds(time);
        _canSlash = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out JunkCar junkcar))
        {
            if(junkcar.CanBeDamaged)
                Slash(junkcar);
        }
    }

}
