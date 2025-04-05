using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using Nenn.InspectorEnhancements.Runtime.Attributes;
using Redcode.Extensions;

namespace Spread.Player.Ladder
{
    using Player.StateMachine;
    using Spread.Interactions;
    using Spread.Ladder;

    public class PlayerLadderController : MonoBehaviour
    {
        private PlayerStateMachineContext _ctx;

        [BoxGroup("Controllers"), SerializeField, InlineProperty] private PlayerLadderEnterController _enterController;
        [BoxGroup("Controllers"), SerializeField, InlineProperty] private PlayerLadderClimbController _climbController;
        [BoxGroup("Controllers"), SerializeField, InlineProperty] private PlayerLadderExitController _exitController;

        [BoxGroup("IK"), SerializeField] private Transform _ikLegL;
        [BoxGroup("IK"), SerializeField] private Transform _ikLegR;
        [BoxGroup("IK"), SerializeField] private Transform _ikArmL;
        [BoxGroup("IK"), SerializeField] private Transform _ikArmR;
        [BoxGroup("IK"), SerializeField] private PlayerLadderThumbController _thumbL;
        [BoxGroup("IK"), SerializeField] private PlayerLadderThumbController _thumbR;

        [BoxGroup("Settings"), SerializeField] private int _ladderStepIndexTopOffset;

        [BoxGroup("Debug"), SerializeField, InlineProperty, NaughtyAttributes.ReadOnly] private PlayerLadderCurrentData _currentData;

        public Ladder CurrentLadder => _currentData.CurrentLadder;
        public bool UsingLadder => _currentData.UsingLadder;

        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;

            _enterController.Setup(_ctx, _currentData, SetIksPos, SyncIks);
            _climbController.Setup(_ctx, _currentData, SetIksPos, SyncIks);
            _exitController.Setup(_ctx, _currentData, SetIksPos, SyncIks);

            _ctx.InputController.Inputs.Keyboard.Move.performed += MoveInput;
            _ctx.InputController.Inputs.Keyboard.Move.canceled += MoveInput;
            _ctx.InteractionsController.OnInteract += Interact;
        }

        private void OnDestroy()
        {
            _ctx.InputController.Inputs.Keyboard.Move.performed -= MoveInput;
            _ctx.InputController.Inputs.Keyboard.Move.canceled -= MoveInput;
        }

        private void Interact(Interactable p_interactable)
        {
            if (CurrentLadder != null && UsingLadder)
            {
                _currentData.CurrentLadder = null;
                _currentData.UsingLadder = true;
            }

            if (CurrentLadder != null || p_interactable is not Ladder ladder) return;

            _currentData.CurrentLadder = ladder;
            _currentData.UsingLadder = false;
        }

        internal void EnterLadder() => _enterController.EnterLadder();

        internal void ClimbLadder() => _climbController.ClimbLadder();

        internal void ExitLadder() => _exitController.ExitLadder();

        internal void ClearUp() => _currentData.ClearUp();

        private void SetIksPos()
        {
            float legOffset = _climbController.StepYLegOffset;
            float armOffset = _climbController.StepYArmOffset;

            if (_currentData.CurrentStep % 2 == 0)
            {
                CurrentLadder.IkLegL.SetPositionY(CurrentLadder.Handles[_currentData.CurrentStep + 1].y + legOffset);
                CurrentLadder.IkLegR.SetPositionY(CurrentLadder.Handles[_currentData.CurrentStep].y + legOffset);
                CurrentLadder.IkArmL.SetPositionY(CurrentLadder.Handles[_currentData.CurrentStep + 4].y + armOffset);
                CurrentLadder.IkArmR.SetPositionY(CurrentLadder.Handles[_currentData.CurrentStep + 5].y + armOffset);
            }
            else
            {
                CurrentLadder.IkLegR.SetPositionY(CurrentLadder.Handles[_currentData.CurrentStep + 1].y + legOffset);
                CurrentLadder.IkLegL.SetPositionY(CurrentLadder.Handles[_currentData.CurrentStep].y + legOffset);
                CurrentLadder.IkArmR.SetPositionY(CurrentLadder.Handles[_currentData.CurrentStep + 4].y + armOffset);
                CurrentLadder.IkArmL.SetPositionY(CurrentLadder.Handles[_currentData.CurrentStep + 5].y + armOffset);
            }
        }

        private void SyncIks()
        {
            _ikLegL.position = CurrentLadder.IkLegL.position;
            _ikLegR.position = CurrentLadder.IkLegR.position;

            _ikArmL.position = CurrentLadder.IkArmL.position;
            _ikArmR.position = CurrentLadder.IkArmR.position;

            _thumbL.UpdateThumb(CurrentLadder.IkThumbTargetL, CurrentLadder.IkThumbHintL);
            _thumbR.UpdateThumb(CurrentLadder.IkThumbTargetR, CurrentLadder.IkThumbHintR);
        }

        private void MoveInput(InputAction.CallbackContext p_ctx)
        {
            Vector2 input = p_ctx.ReadValue<Vector2>();
            _currentData.ClimbDirection = input.y > 0 ? 1
                : input.y < 0 ? -1
                : 0;
        }
    }
}
