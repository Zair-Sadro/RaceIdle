using System.Collections;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    [SerializeField] private float activeHammerCollTime;
    [SerializeField] private float delayBeforeDamage;
    [SerializeField] private int damage = 1;

    private PlayerController controller => InstantcesContainer.Instance.PlayerController;
    public int Damage => damage;

    public void SetDamage(int d) 
    {
        damage = d;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Destroyable")
            controller.Animator.SetBool("Slash", true);
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Destroyable")
            controller.Animator.SetBool("Slash", false);
    }

}
