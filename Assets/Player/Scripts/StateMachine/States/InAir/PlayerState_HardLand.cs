using System;
using UnityEngine;
using KosciachTools.Delay;

namespace Player
{
    public class PlayerState_HardLand : PlayerBaseState
    {
        private bool _finished;

        public PlayerState_HardLand(PlayerStateMachineContext p_ctx) : base(p_ctx) { }

        public override void InitializeSubState()
        {

        }


        public override void Enter()
        {
            _ctx.AnimatorController.Land(true);
            KosciachDelay.Delay(1.8f, () => { _finished = true; });
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            _finished = false;
            _ctx.MovementController.MaxInAirSpeed = 0;
        }

        public override Type GetNextState()
        {
            if(_finished)
            {
                return typeof(PlayerState_Idle);
            }

            return GetType();
        }
    }
}