using UnityEngine;

namespace Spread.Player.Animating
{
    using StateMachine;

    public class BehaviourChange : StateMachineBehaviour
    {
        private PlayerStateMachine _player;
        [SerializeField] private int _layerIndex;
        [SerializeField] private string _name;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_player == null)
            {
                _player = animator.transform.parent.GetComponent<PlayerStateMachine>();
            }

            _player.Ctx.AnimatorController.StateEnter(_layerIndex, _name);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_player == null)
            {
                _player = animator.transform.parent.GetComponent<PlayerStateMachine>();
            }

            _player.Ctx.AnimatorController.StateExit(_layerIndex, _name);
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
}
