using UnityEngine;
using UnityEngine.InputSystem;

namespace Spread.Player.Ladder
{
    using Player.StateMachine;
    using Spread.Interactions;
    using Spread.Ladder;

    public class PlayerLadderController : MonoBehaviour
    {
        private PlayerStateMachineContext _ctx;

        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;

            _ctx.InteractionsController.OnInteract += Interaction;
            
            _ctx.InputController.Inputs.Keyboard.Move.performed += MoveInput;
            _ctx.InputController.Inputs.Keyboard.Move.canceled += MoveInput;
        }

        private void OnDestroy()
        {
            _ctx.InputController.Inputs.Keyboard.Move.performed -= MoveInput;
            _ctx.InputController.Inputs.Keyboard.Move.canceled -= MoveInput;
        }

        private void MoveInput(InputAction.CallbackContext p_ctx)
        {

        }

        private void Interaction(Interactable p_interactable)
        {
            if (p_interactable is Ladder ladder)
            {
                Debug.Log(ladder);
            }
        }
    }
}
