using System;

namespace Spread.Player.StateMachine
{
    public class ExitLadderState : PlayerBaseState
    {
        protected override void OnEnter()
        {
            _ctx.LadderController.ExitLadder();
        }

        protected override void OnUpdate()
        {

        }

        protected override void OnExit()
        {

        }

        internal override Type GetNextState()
        {
            if (_ctx.LadderController.CurrentLadder == null)
            {
                return typeof(IdleState);
            }

            return GetType();
        }
    }
}