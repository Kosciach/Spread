using System;
using DG.Tweening;
using SaintsField;
using UnityEngine;
using SaintsField.Playa;
using Spread.Player.Animating;
using Spread.Player.Camera;
using Spread.Player.Collisions;
using Spread.Player.Gravity;
using Spread.Player.Interactions;
using Spread.Player.Ladder;
using Spread.Player.Movement;
using Spread.Tools;

namespace Spread.Player.StateMachine
{
    public class EnterLadderState : PlayerBaseState
    {
        private PlayerLadderController _ladderController;
        private PlayerAnimatorController _animatorController;
        private PlayerMovementController _movementController;
        private PlayerGravityController _gravityController;
        private PlayerColliderController _colliderController;
        private PlayerInteractionsController _interactionsController;
        private PlayerCameraController _cameraController;
        
        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private LadderState _ladderState;
        
        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private float _verticalRotation;
        [SerializeField] private float _horizontalRotationRange = 75;
        [LayoutStart("Settings/Durations", ELayout.TitleBox)]
        [LayoutStart("Settings/Durations/Top", ELayout.TitleBox | ELayout.Foldout), SaintsRow(inline: true)]
        [SerializeField] private EnterLadderDurations _topDurations;
        [LayoutStart("Settings/Durations/Bottom", ELayout.TitleBox | ELayout.Foldout), SaintsRow(inline: true)]
        [SerializeField] private EnterLadderDurations _bottomDurations;

        private bool _readyToClimb;
        

        protected override void OnSetup()
        {
            _ladderController = _ctx.GetController<PlayerLadderController>();
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _colliderController = _ctx.GetController<PlayerColliderController>();
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _cameraController = _ctx.GetController<PlayerCameraController>();
        }
        
        protected override void OnEnter()
        {
            //Prep values
            Spread.Ladder.Ladder ladder = _ladderController.CurrentLadder;
            int closestRungIndex = ladder.GetClosestRungIndex(_ctx.Transform.position, _ladderState.MaxRungIndexOffset);
            _readyToClimb = false;

            EnterLadderDurations durations = ladder.IsPlayerTop(_ctx.Transform.position)
                ? _topDurations
                : _bottomDurations;
            
            //Transition to ladder anims
            _animatorController.LadderEnter(false);
            _animatorController.ToggleFootIk(false);
            _animatorController.SetAnimatorLayerWeight(AnimatorLayer.InAir, 0f);
            Helpers.SimpleTimer(durations.RotateY, () =>
            {
                _animatorController.SetAnimatorIkRigWeight(AnimatorIkRig.Ladder, 1, durations.EnableLadderRig);
            });
            
            //Root motion - off
            _animatorController.ToggleRootMotion(false);
            _movementController.RootMotionMove = false;
            
            //Gravity - off
            _gravityController.ToggleGravity(false);
            _gravityController.ToggleIkCrouch(false);
            _colliderController.ToggleCollision(false);
            
            //Unselect ladder
            _interactionsController.SetInteractable(null);
            
            //Set IK
            _ladderController.SetStartIkPos(closestRungIndex);
            
            //Move to ladder
            Vector3 attachPoint = ladder.AttachPoints[closestRungIndex];
            _ctx.Transform.DOMove(attachPoint, durations.MoveToLadder);
            _ladderState.CurrentRangIndex = closestRungIndex;
            
            //Rotate to Ladder
            _cameraController.RotToXAxis(_verticalRotation, durations.RotateX);
            _cameraController.RotToYAxis(ladder.transform.eulerAngles.y, durations.RotateY);
            _ctx.RotToYAxis(ladder.transform.eulerAngles.y, durations.RotateY, () =>
            {
                //Set Camera MinMax
                _cameraController.ToggleWrap(false);
                _cameraController.SetMinMax(ladder.transform.eulerAngles.y, _horizontalRotationRange);
            
                _readyToClimb = true;
            });
        }

        protected override void OnUpdate()
        {
            
        }

        protected override void OnExit()
        {
            _readyToClimb = false;
        }

        internal override Type GetNextState()
        {
            if (_readyToClimb)
            {
                return typeof(LadderState);
            }
            
            return GetType();
        }
    }
    
    [System.Serializable]
    internal class EnterLadderDurations
    {
        [SerializeField] private float _enableLadderRig;
        [SerializeField] private float _moveToLadder;
        [SerializeField] private float _rotateX;
        [SerializeField] private float _rotateY;

        internal float EnableLadderRig => _enableLadderRig;
        internal float MoveToLadder => _moveToLadder;
        internal float RotateX => _rotateX;
        internal float RotateY => _rotateY;
    }
}