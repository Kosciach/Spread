using UnityEngine;

namespace Spread.Player.Animating
{
    public class BehaviourChange : StateMachineBehaviour
    {
        private PlayerAnimatorController _playerAnimatorController;
        
        [SerializeField] private int _layerIndex;
        [SerializeField] private string _name;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_playerAnimatorController == null)
            {
                _playerAnimatorController = animator.transform.parent.GetComponent<PlayerAnimatorController>();
            }

            _playerAnimatorController.StateEnter(_layerIndex, _name);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_playerAnimatorController == null)
            {
                _playerAnimatorController = animator.transform.parent.GetComponent<PlayerAnimatorController>();
            }

            _playerAnimatorController.StateExit(_layerIndex, _name);
        }
    }
}
