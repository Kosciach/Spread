using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerState_Run : PlayerBaseState
    {

        public PlayerState_Run(PlayerStateMachineContext p_ctx) : base(p_ctx) { }

        public override void InitializeSubState()
        {

        }


        public override void Enter()
        {
            _ctx.MovementController.IsCrouch = false;
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
            if (!_ctx.InputController.IsRun || _ctx.InputController.MoveInputVector.y <= 0)
            {
                return typeof(PlayerState_Walk);
            }
            if (!_ctx.InputController.IsMoving)
            {
                return typeof(PlayerState_Idle);
            }

            return GetType();
        }
    }
}