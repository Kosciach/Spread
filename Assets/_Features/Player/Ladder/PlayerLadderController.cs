using DG.Tweening;
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

        private Ladder _currentLadder;
        internal Ladder CurrentLadder => _currentLadder;
        
        
        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;

            _ctx.InteractionsController.OnInteract += Interaction;
        }
        
        private void Interaction(Interactable p_interactable)
        {
            if (p_interactable is Ladder ladder)
            {
                _currentLadder = ladder;
            }
        }
    }
}
