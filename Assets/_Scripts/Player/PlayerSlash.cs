using System.Collections;
using UnityEngine;

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


        }
            
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Destroyable") 
        {
            controller.Animator.SetBool("Attacking", false);
            _hasEnemy = false;
            StopAttack();
        }
           


    }

    private void Attack()
    {
    StartCoroutine(AttackingCor());
    }
    IEnumerator AttackingCor()
    {
        while (_hasEnemy)
        {
            var r = Random.Range(1, attacksTypesCount);
            controller.Animator.SetInteger("RandomHit", r);
            yield return new WaitForSeconds(0.9f);
        }

    }
    private void StopAttack()
    {
        controller.Animator.SetInteger("RandomHit", -1);
        StopAllCoroutines();

    }
}
