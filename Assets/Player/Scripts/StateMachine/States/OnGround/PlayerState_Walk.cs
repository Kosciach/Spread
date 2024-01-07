using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerState_Walk : PlayerBaseState
    {

        public PlayerState_Walk(PlayerStateMachineContext p_ctx) : base(p_ctx) { }

        public override void InitializeSubState()
        {
            _currentSubState = _ctx.States[typeof(PlayerState_Crouch)];
        }


        public override void Enter()
        {

        }

        public override void Update()
        {
            _ctx.CameraController.Look();
            _ctx.MovementController.NormalMove();
        }

        public override void Exit()
        {

        }

        public override Type GetNextState()
        {
            if (_ctx.SlopeController.ShouldSlide)
            {
                return typeof(PlayerState_SlopeSlide);
            }
            if (!_ctx.GroundCheck.IsGrounded)
            {
                return typeof(PlayerState_Fall);
            }
            if (_ctx.MovementController.IsJump)
            {
                return typeof(PlayerState_Jump);
            }
            if (!_ctx.InputController.IsMoving)
            {
                return typeof(PlayerState_Idle);
            }
            if(_ctx.InputController.IsRun && _ctx.InputController.MoveInputVector.y > 0)
            {
                return typeof(PlayerState_Run);
            }

            return GetType();
        }
    }
}