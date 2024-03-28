using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    public string[] attackNames;
    private PlayerSlash _playerAttack => InstantcesContainer.Instance.PlayerSlasher;
    public float triggerTime = 0.9f; // Время, в течение которого можно запустить новый триггер перед завершением анимации

    private bool triggerSent = false;
    private float animationLength; 
    private float timeElapsed; 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationLength = stateInfo.length; 
        timeElapsed = 0f;
        triggerSent = false; // Сбрасываем флаг отправки триггера
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_playerAttack.HasEnemy) 
        {
            animator.SetBool("Attacking", false);
            triggerSent = true;
            for (int i = 0; i < attackNames.Length; i++)
            {
                animator.ResetTrigger(attackNames[i]);
            }

        }
        timeElapsed += Time.deltaTime; 

        if (_playerAttack.HasEnemy && !triggerSent && timeElapsed >= triggerTime * animationLength)
        {
            SendRandomAttackTrigger(animator);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        triggerSent = false; 
    }

    private void SendRandomAttackTrigger(Animator animator)
    {

        int randomIndex = Random.Range(0, attackNames.Length); 
        animator.SetTrigger(attackNames[randomIndex]); 
        triggerSent = true; 
    }
}
