using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMPlayerMovement : StateMachineBehaviour
{

    [Zenject.Inject] private PlayerController _player;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Speed", _player.CurrentSpeed);
    }
   
}
