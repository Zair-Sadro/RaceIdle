using System.Collections;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    [SerializeField] private float activeHammerCollTime;
    [SerializeField] private float delayBeforeDamage;

   private PlayerController controller=>InstantcesContainer.Instance.PlayerController;

    private bool _canSlash = true;
    private void Slash(JunkCar car)
    {
        if (_canSlash)
            StartCoroutine(SlashRoutine(activeHammerCollTime));
    }

    private IEnumerator SlashRoutine(float time)
    {
        _canSlash = false;


        yield return new WaitForSeconds(time);
        _canSlash = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out JunkCar junkcar))
        {
            controller.Animator.SetBool("Slash", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out JunkCar junkcar))
        {
            controller.Animator.SetBool("Slash", false);
        }
    }

}
