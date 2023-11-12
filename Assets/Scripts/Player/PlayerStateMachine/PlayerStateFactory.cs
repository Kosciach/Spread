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


        public PlayerBaseState Jump()
        {
            return new PlayerState_Jump(_playerStateMachine, this);
        }
        public PlayerBaseState Fall()
        {
            return new PlayerState_Fall(_playerStateMachine, this);
        }
        public PlayerBaseState Land()
        {
            return new PlayerState_Land(_playerStateMachine, this);
        }
        public PlayerBaseState HardLanding()
        {
            return new PlayerState_HardLanding(_playerStateMachine, this);
        }


        public PlayerBaseState Crouch()
        {
            return new PlayerState_Crouch(_playerStateMachine, this);
        }
    }
}