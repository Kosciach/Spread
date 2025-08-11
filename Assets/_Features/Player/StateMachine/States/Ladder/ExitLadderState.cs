using System;
using DG.Tweening;
using UnityEngine;
using SaintsField;
using SaintsField.Playa;

namespace Spread.Player.StateMachine
{
    using Ladder;
    using Animating;
    using Camera;
    using Movement;
    using Gravity;
    using Collisions;
    
    public class ExitLadderState : PlayerBaseState
    {
        private PlayerLadderController _ladderController;
        private PlayerAnimatorController _animatorController;
        private PlayerCameraController _cameraController;
        private PlayerMovementController _movementController;
        private PlayerGravityController _gravityController;
        private PlayerColliderController _colliderController;
        
        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private LadderState _ladderState;
        
        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private float _verticalRotation;
        
        [LayoutStart("Settings/Durations", ELayout.TitleBox)]
        [LayoutStart("Settings/Durations/Top", ELayout.TitleBox | ELayout.Foldout), SaintsRow(inline: true)]
        [SerializeField] private ExitLadderDurations _topDurations;
        
        [LayoutStart("Settings/Durations/Bottom", ELayout.TitleBox | ELayout.Foldout), SaintsRow(inline: true)]
        [SerializeField] private ExitLadderDurations _bottomDurations;

        private bool _exitFinished;
        internal int ExitDirection;

        protected override void OnSetup()
        {
            _ladderController = _ctx.GetController<PlayerLadderController>();
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _cameraController = _ctx.GetController<PlayerCameraController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _colliderController = _ctx.GetController<PlayerColliderController>();
        }

        protected override void OnEnter()
        {
            // Prep values
            Spread.Ladder.Ladder ladder = _ladderController.CurrentLadder;
            _exitFinished = false;

            ExitLadderDurations durations = _bottomDurations;
            Vector3 exitPoint = ladder.BottomExitPoint;
            bool exitTop = ExitDirection == 1;
            if (exitTop)
            {
                durations = _topDurations;
                exitPoint = ladder.TopExitPoint;
            }

            // Disable ladder anims
            _animatorController.LadderExit(false);
            _animatorController.SetAnimatorIkRigWeight(AnimatorIkRig.Ladder, 0, durations.DisableLadderRig);

            // Reset rotation
            _cameraController.RotToXAxis(_verticalRotation, durations.RotateX);
            _cameraController.RotToYAxis(ladder.transform.eulerAngles.y, durations.RotateY);
            _ctx.RotToYAxis(ladder.transform.eulerAngles.y, durations.RotateY);

            // Move to exit point
            _ctx.Transform.DOMove(exitPoint, durations.MoveToExitPoint).OnComplete(() =>
            {
                // Reset Camera MinMax
                _cameraController.ResetMinMax();
                _cameraController.ToggleWrap(true);

                // Root motion - on
                _animatorController.ToggleRootMotion(true);
                _movementController.RootMotionMove = true;

                // Gravity - on
                _gravityController.ToggleGravity(true);
                _colliderController.ToggleCollision(true);

                // Feet Ik - on
                _animatorController.ToggleFootIk(true);

                // Set exit
                _exitFinished = true;
            });

            // Clear ladder controller
            _ladderController.Clear();
        }

        protected override void OnUpdate()
        {
            _movementController.RestrainNormalMovement();
        }

        protected override void OnExit()
        {
            _gravityController.ToggleIkCrouch(true);
            _exitFinished = true;
        }

        internal override Type GetNextState()
        {
            if (_exitFinished)
            {
                return typeof(IdleState);
            }

            return GetType();
        }
    }

    [Serializable]
    internal class ExitLadderDurations
    {
        [SerializeField] private float _disableLadderRig;
        [SerializeField] private float _moveToExitPoint;
        [SerializeField] private float _rotateX;
        [SerializeField] private float _rotateY;
        
        internal float DisableLadderRig => _disableLadderRig;
        internal float MoveToExitPoint => _moveToExitPoint;
        internal float RotateX => _rotateX;
        internal float RotateY => _rotateY;
    }
}