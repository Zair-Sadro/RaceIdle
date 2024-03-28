using System.Collections;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    [SerializeField] private Collider[] _fightColliders;

    [SerializeField] private int damage = 1;
    [SerializeField] private string[] _attackNames;

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

        }
            
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Destroyable") 
        {
            controller.Animator.SetBool("Attacking", false);
            _hasEnemy = false;
        }
           


    }

    private void Attack()
    {

    }
    private bool isAttacking;
    IEnumerator AttackingCor()
    {
        
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        var r = Random.Range(0, _attackNames.Length);
        controller.Animator.SetTrigger(_attackNames[r]);
    }
    private void StopAttack()
    {
        var anim = controller.Animator;

        for (int i = 0; i < _attackNames.Length; i++)
        {
            anim.ResetTrigger(_attackNames[i]);
        }
        //  anim.SetTrigger("StopFight");

    }
}
