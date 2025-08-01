using System;
using DG.Tweening;
using UnityEngine;
using SaintsField;
using SaintsField.Playa;

namespace Spread.Player.StateMachine
{
    public class ExitLadderState : PlayerBaseState
    {
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
        
        protected override void OnEnter()
        {
            //Prep values
            Spread.Ladder.Ladder ladder = _ctx.LadderController.CurrentLadder;
            _exitFinished = false;

            ExitLadderDurations durations = _bottomDurations;
            Vector3 exitPoint = ladder.BottomExitPoint;
            bool exitTop = ExitDirection == 1;
            if (exitTop)
            {
                durations = _topDurations;
                exitPoint = ladder.TopExitPoint;
            }
            
            //Disable ladder anims
            _ctx.AnimatorController.LadderExit(false);
            _ctx.AnimatorController.SetLadderRig(0, durations.DisableLadderRig);
            
            //Reset rotation
            _ctx.CameraController.RotToXAxis(_verticalRotation, durations.RotateX);
            _ctx.CameraController.RotToYAxis(ladder.transform.eulerAngles.y, durations.RotateY);
            _ctx.RotToYAxis(ladder.transform.eulerAngles.y, durations.RotateY);

            //Move to exit point
            _ctx.Transform.DOMove(exitPoint, durations.MoveToExitPoint).OnComplete(() =>
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