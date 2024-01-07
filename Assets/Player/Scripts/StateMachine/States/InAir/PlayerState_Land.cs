using System;
using UnityEngine;

namespace Player
{
    public class PlayerState_Land : PlayerBaseState
    {

        public PlayerState_Land(PlayerStateMachineContext p_ctx) : base(p_ctx) { }

        public override void InitializeSubState()
        {

        }


        public override void Enter()
        {
            _ctx.AnimatorController.Land(false);
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            _ctx.MovementController.MaxInAirSpeed = 0;
        }

        public override Type GetNextState()
        {
            return typeof(PlayerState_Idle);
        }
    }
}