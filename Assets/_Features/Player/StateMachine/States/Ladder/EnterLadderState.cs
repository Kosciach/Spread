using System;

namespace Spread.Player.StateMachine
{
    public class EnterLadderState : PlayerBaseState
    {
        protected override void OnEnter()
        {
            _ctx.LadderController.EnterLadder();
        }

        protected override void OnUpdate()
        {

        }

        protected override void OnExit()
        {

        }

        internal override Type GetNextState()
        {
            if(_ctx.LadderController.UsingLadder)
            {
                return typeof(LadderState);
            }

            return GetType();
        }
    }
}