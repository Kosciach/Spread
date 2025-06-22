using System;
using DG.Tweening;

namespace Spread.Player.StateMachine
{
    public class LadderState : PlayerBaseState
    {
        protected override void OnEnter()
        {

        }

        protected override void OnUpdate()
        {

        }

        protected override void OnExit()
        {

        }

        internal override Type GetNextState()
        {
            if (_ctx.GravityController.IsJump)
            {
                return typeof(JumpState);
            }
            
            return GetType();
        }
    }
}