using System;

namespace Player
{
    public class PlayerState_Empty : PlayerBaseState
    {

        public PlayerState_Empty(PlayerStateMachineContext p_ctx) : base(p_ctx) { }


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
            if(_ctx.MovementController.IsCrouch)
            {
                return typeof(PlayerState_Crouch);
            }
            return GetType();
        }
    }
}