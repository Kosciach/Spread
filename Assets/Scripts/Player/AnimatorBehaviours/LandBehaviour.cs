using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerStateMachine playerStateMachine = animator.GetComponent<PlayerStateMachine>();

        playerStateMachine.HandsCameraController.EnableController.ToggleHandsCamera(true);
        playerStateMachine.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, true, 6);
        playerStateMachine.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, true, 6);
        playerStateMachine.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, true, 6);
        playerStateMachine.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.Head, true, 6);

        playerStateMachine.WasHardLanding = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
