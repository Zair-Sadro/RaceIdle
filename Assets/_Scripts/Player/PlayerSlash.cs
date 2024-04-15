using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerSlash : MonoBehaviour
{
    [SerializeField] private Collider[] _fightColliders;

    [SerializeField] private int damage = 1;
    [SerializeField] private int attacksTypesCount;

    private bool _hasEnemy;
    private PlayerController controller => InstantcesContainer.Instance.PlayerController;
    public int Damage => damage;
    public bool HasEnemy => _hasEnemy;

    public void SetDamage(int d)
    {
        damage = d;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Destroyable")
        {
            controller.Animator.SetBool("Attacking", true);
            _hasEnemy = true;
            Attack();

            print("enter");
        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Destroyable")
        {
            controller.Animator.SetBool("Attacking", false);
            _hasEnemy = false;
            StopAttack();
            print("exit");
        }



    }

    private void Attack()
    {
        StartCoroutine(AttackingCor());
    }

    IEnumerator AttackingCor()
    {
        ActivateColliders(true);
        while (_hasEnemy)
        {
            var r = Random.Range(1, attacksTypesCount);
            controller.Animator.SetInteger("RandomHit", r);
            yield return new WaitForSeconds(0.9f);
        }

    }

    private void StopAttack()
    {
        ActivateColliders(false);
        controller.Animator.SetInteger("RandomHit", -1);
        StopAllCoroutines();

    }
    private void ActivateColliders(bool value)
    {
        for (int i = 0; i < _fightColliders.Length; i++)
        {
            _fightColliders[i].enabled = value;
        }
    }
}
