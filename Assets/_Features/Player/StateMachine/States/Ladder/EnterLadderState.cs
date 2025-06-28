using System;
using DG.Tweening;
using UnityEngine;

namespace Spread.Player.StateMachine
{
    public class EnterLadderState : PlayerBaseState
    {
        [SerializeField] private LadderState _ladderState;
        [SerializeField] private float _moveToRungDuration;
        [SerializeField] private float _rotateToLadderDuration;

        private bool _readyToClimb;
        
        protected override void OnEnter()
        {
            _readyToClimb = false;
            
            //Transition to ladder anims
            _ctx.AnimatorController.LadderEnter(false);
            _ctx.AnimatorController.ToggleFootIk(false);
            _ctx.AnimatorController.SetInAirLayer(0);
            _ctx.AnimatorController.SetLadderRig(1);
            
            //Root motion - off
            _ctx.AnimatorController.ToggleRootMotion(false);
            _ctx.MovementController.RootMotionMove = true;
            
            //Gravity - off
            _ctx.GravityController.ToggleGravity(false);
            
            //Unselect ladder
            _ctx.InteractionsController.SetInteractable(null);

            Spread.Ladder.Ladder ladder = _ctx.LadderController.CurrentLadder;
            
            //Move to rung
            int closestRungIndex = ladder.GetClosestRungIndex(_ctx.Transform.position);
            Vector3 attachPoint = ladder.AttachPoints[closestRungIndex];
            _ctx.Transform.DOMove(attachPoint, _moveToRungDuration);
            _ladderState.CurrentRangIndex = closestRungIndex;
            
            //Rotate to Ladder
            _ctx.CameraController.RotToYAxis(ladder.transform.eulerAngles.y, _rotateToLadderDuration);
            _ctx.RotToYAxis(ladder.transform.eulerAngles.y, _rotateToLadderDuration, () => { _readyToClimb = true; });
            
            //Set Camera MinMax
            _ctx.CameraController.SetMinMax(ladder.transform.eulerAngles.y, 75);
            _ctx.CameraController.ToggleWrap(false);
            
            //Set IK
            _ctx.LadderController.SetIkPos(closestRungIndex);
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