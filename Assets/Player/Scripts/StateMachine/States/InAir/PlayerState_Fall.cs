using System;
using UnityEngine;

namespace Player
{
    public class PlayerState_Fall : PlayerBaseState
    {

        public PlayerState_Fall(PlayerStateMachineContext p_ctx) : base(p_ctx) { }

        public override void InitializeSubState()
        {

        }


        public override void Enter()
        {
            _ctx.MovementController.IsCrouch = false;
            _ctx.MovementController.SaveMaxInAirSpeed();
            _ctx.AnimatorController.Fall();
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
            if (_ctx.GroundCheck.IsGrounded)
            {
                return _ctx.GravityController.CurrentGravityForce < -9.1f ? typeof(PlayerState_HardLand) : typeof(PlayerState_Land);
            }
            return GetType();
        }
    }
}