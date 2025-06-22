using System;

namespace Spread.Player.StateMachine
{
    public class ExitLadderState : PlayerBaseState
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
            return GetType();
        }
    }
}