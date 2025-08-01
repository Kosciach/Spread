using System;
using DG.Tweening;
using SaintsField;
using UnityEngine;
using SaintsField.Playa;
using Spread.Tools;

namespace Spread.Player.StateMachine
{
    public class EnterLadderState : PlayerBaseState
    {
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
        
        protected override void OnEnter()
        {
            //Prep values
            Spread.Ladder.Ladder ladder = _ctx.LadderController.CurrentLadder;
            int closestRungIndex = ladder.GetClosestRungIndex(_ctx.Transform.position, _ladderState.MaxRungIndexOffset);
            _readyToClimb = false;

            EnterLadderDurations durations = ladder.IsPlayerTop(_ctx.Transform.position)
                ? _topDurations
                : _bottomDurations;
            
            //Transition to ladder anims
            _ctx.AnimatorController.LadderEnter(false);
            _ctx.AnimatorController.ToggleFootIk(false);
            _ctx.AnimatorController.SetInAirLayer(0);
            Helpers.SimpleTimer(durations.RotateY, () =>
            {
                _ctx.AnimatorController.SetLadderRig(1, durations.EnableLadderRig);
            });
            
            //Root motion - off
            _ctx.AnimatorController.ToggleRootMotion(false);
            _ctx.MovementController.RootMotionMove = false;
            
            //Gravity - off
            _ctx.GravityController.ToggleGravity(false);
            _ctx.GravityController.ToggleIkCrouch(false);
            _ctx.ColliderController.ToggleCollision(false);
            
            //Unselect ladder
            _ctx.InteractionsController.SetInteractable(null);
            
            //Set IK
            _ctx.LadderController.SetStartIkPos(closestRungIndex);
            
            //Move to ladder
            Vector3 attachPoint = ladder.AttachPoints[closestRungIndex];
            _ctx.Transform.DOMove(attachPoint, durations.MoveToLadder);
            _ladderState.CurrentRangIndex = closestRungIndex;
            
            //Rotate to Ladder
            _ctx.CameraController.RotToXAxis(_verticalRotation, durations.RotateX);
            _ctx.CameraController.RotToYAxis(ladder.transform.eulerAngles.y, durations.RotateY);
            _ctx.RotToYAxis(ladder.transform.eulerAngles.y, durations.RotateY, () =>
            {
                //Set Camera MinMax
                _ctx.CameraController.ToggleWrap(false);
                _ctx.CameraController.SetMinMax(ladder.transform.eulerAngles.y, _horizontalRotationRange);
            
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