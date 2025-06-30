using System;
using DG.Tweening;
using UnityEngine;
using SaintsField;
using SaintsField.Playa;
using Spread.Tools;

namespace Spread.Player.StateMachine
{
    public class ExitLadderState : PlayerBaseState
    {
        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private LadderState _ladderState;
        
        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private float _verticalRotation;
        [LayoutStart("Settings/Durations", ELayout.TitleBox)]
        [SerializeField] private float _disableLadderRigDuration;
        [SerializeField] private float _moveToExitPointDuration;
        [SerializeField] private float _rotateXDuration;
        [SerializeField] private float _rotateYDuration;

        private bool _exitFinished;
        
        protected override void OnEnter()
        {
            _exitFinished = false;
            
            //Prep values
            Spread.Ladder.Ladder ladder = _ctx.LadderController.CurrentLadder;
            
            //Disable ladder anims
            _ctx.AnimatorController.LadderExit(false);
            _ctx.AnimatorController.SetLadderRig(0, _disableLadderRigDuration);
            
            //Reset rotation
            _ctx.CameraController.RotToXAxis(_verticalRotation, _rotateXDuration);
            _ctx.CameraController.RotToYAxis(ladder.transform.eulerAngles.y, _rotateYDuration);
            _ctx.RotToYAxis(ladder.transform.eulerAngles.y, _rotateYDuration);

            //Move to exit point
            _ctx.Transform.DOMove(ladder.BottomExitPoint, _moveToExitPointDuration);
            Helpers.SimpleTimer(_moveToExitPointDuration, () =>
            {
                //Reset Camera MinMax
                _ctx.CameraController.ResetMinMax();
                _ctx.CameraController.ToggleWrap(true);

                //Root motion - on
                _ctx.AnimatorController.ToggleRootMotion(true);
                _ctx.MovementController.RootMotionMove = true;

                //Gravity - on
                _ctx.GravityController.ToggleGravity(true);
                _ctx.ColliderController.ToggleCollision(true);

                //Feet Ik - on
                _ctx.AnimatorController.ToggleFootIk(true);
                
                //Set exit
                _exitFinished = true;
            });
            
            //Clear ladder controller
            _ctx.LadderController.Clear();
        }

        protected override void OnUpdate()
        {
            _ctx.MovementController.RestrainNormalMovement();
        }

        protected override void OnExit()
        {
            _ctx.GravityController.ToggleIkCrouch(true);
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
}