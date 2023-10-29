using UnityEngine;

namespace PlayerStateMachineSystem
{
    public class PlayerStateFactory
    {
        private PlayerStateMachine _playerStateMachine;

        public PlayerStateFactory(PlayerStateMachine playerStateMachine)
        {
            _playerStateMachine = playerStateMachine;
        }


        public PlayerBaseState Idle()
        {
            return new PlayerState_Idle(_playerStateMachine, this);
        }
        public PlayerBaseState Walk()
        {
            return new PlayerState_Walk(_playerStateMachine, this);
        }
        public PlayerBaseState Run()
        {
            return new PlayerState_Run(_playerStateMachine, this);
        }
    }
}