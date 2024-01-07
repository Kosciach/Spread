using System;
using UnityEngine;

namespace Player
{
    public class PlayerState_Crouch : PlayerBaseState
    {

        public PlayerState_Crouch(PlayerStateMachineContext p_ctx) : base(p_ctx) { }


        public override void Enter()
        {

        }

        public override void Update()
        {

        }

        public override void Exit()
        {

        }

        public override Type GetNextState()
        {
            if (!_ctx.MovementController.IsCrouch)
            {
                return typeof(PlayerState_Empty);
            }
            return GetType();
        }
    }
}