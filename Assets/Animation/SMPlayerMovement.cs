using UnityEngine;

public class SMPlayerMovement : StateMachineBehaviour
{
    private PlayerController _player => InstantcesContainer.Instance.PlayerController;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Speed", _player.CurrentSpeed);

    }


}
