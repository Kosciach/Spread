using System;
using DG.Tweening;
using UnityEngine;
using SaintsField;
using SaintsField.Playa;
using Spread.Tools;

namespace Spread.Player.StateMachine
{
    public class EnterLadderState : PlayerBaseState
    {
        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private LadderState _ladderState;
        
        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private float _horizontalRotation;
        [LayoutStart("Settings/Durations", ELayout.TitleBox)]
        [SerializeField] private float _setLadderRigDuration;
        [SerializeField] private float _moveToLadderDuration;
        [SerializeField] private float _rotateXDuration;
        [SerializeField] private float _rotateYDuration;

        private bool _readyToClimb;
        
        protected override void OnEnter()
        {
            _readyToClimb = false;
            
            //Transition to ladder anims
            _ctx.AnimatorController.LadderEnter(false);
            _ctx.AnimatorController.ToggleFootIk(false);
            _ctx.AnimatorController.SetInAirLayer(0);
            Helpers.SimpleTimer(_rotateYDuration, () =>
            {
                _ctx.AnimatorController.SetLadderRig(1, _setLadderRigDuration);
            });
            
            //Root motion - off
            _ctx.AnimatorController.ToggleRootMotion(false);
            _ctx.MovementController.RootMotionMove = true;
            
            //Gravity - off
            _ctx.GravityController.ToggleGravity(false);
            _ctx.ColliderController.ToggleCollision(false);
            
            //Unselect ladder
            _ctx.InteractionsController.SetInteractable(null);
            
            //Prep values
            Spread.Ladder.Ladder ladder = _ctx.LadderController.CurrentLadder;
            int closestRungIndex = ladder.GetClosestRungIndex(_ctx.Transform.position);
            
            //Set IK
            _ctx.LadderController.SetStartIkPos(closestRungIndex);
            
            //Move to ladder
            Vector3 attachPoint = ladder.AttachPoints[closestRungIndex];
            _ctx.Transform.DOMove(attachPoint, _moveToLadderDuration);
            _ladderState.CurrentRangIndex = closestRungIndex;
            
            //Rotate to Ladder
            _ctx.CameraController.RotToXAxis(_horizontalRotation, _rotateXDuration);
            _ctx.CameraController.RotToYAxis(ladder.transform.eulerAngles.y, _rotateYDuration);
            _ctx.RotToYAxis(ladder.transform.eulerAngles.y, _rotateYDuration, () => { _readyToClimb = true; });
            
            //Set Camera MinMax
            _ctx.CameraController.SetMinMax(ladder.transform.eulerAngles.y, 75);
            _ctx.CameraController.ToggleWrap(false);
        }

        protected override void OnUpdate()
        {
            
        }

        protected override void OnExit()
        {

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
}