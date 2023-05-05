using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchController
{
    private PlayerStateMachine _stateMachine;
    private PlayerGravityController _gravityController;
    private PlayerInputController _inputController;
    private PlayerClimbController _climbController;
    private PlayerJumpController _jumpController;
    private SwitchToClass _switchTo; public SwitchToClass SwitchTo { get { return _switchTo; } }


    public PlayerSwitchController(PlayerStateMachine stateMachine, PlayerGravityController gravityController, PlayerInputController inputController, PlayerClimbController climbController, PlayerJumpController jumpController)
    {
        _stateMachine = stateMachine;
        _gravityController = gravityController;
        _inputController = inputController;
        _climbController = climbController;
        _jumpController = jumpController;
        _switchTo = new SwitchToClass(_stateMachine, _gravityController, _inputController, _climbController, _jumpController, this);
    }






    public bool IsSwitch(PlayerStateMachine.SwitchEnum stateSwitch)
    {
        return _stateMachine.StateSwitch == stateSwitch;
    }




    public class SwitchToClass
    {
        private PlayerStateMachine _stateMachine;
        private PlayerGravityController _gravityController;
        private PlayerInputController _inputController;
        private PlayerClimbController _climbController;
        private PlayerJumpController _jumpController;
        private PlayerSwitchController _switchController;
        public SwitchToClass(PlayerStateMachine stateMachine, PlayerGravityController gravityController, PlayerInputController inputController, PlayerClimbController climbController, PlayerJumpController jumpController, PlayerSwitchController switchController)
        {
            _stateMachine = stateMachine;
            _gravityController = gravityController;
            _inputController = inputController;
            _climbController = climbController;
            _switchController = switchController;
            _jumpController = jumpController;
        }





        public void Idle()
        {
            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Idle;
        }
        public void Walk()
        {
            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Walk;
        }
        public void Run()
        {
            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Run;
        }



        public void Jump()
        {
            bool readyToJump = _gravityController.GetIsGrounded() && !_jumpController.CheckAboveObsticle() && _jumpController.GetJumpReloaded();
            if (!readyToJump) return;


            //Dash();

            //if (_stateMachine.MovementController.IsDashDirection()) return;

            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Jump;



            if (!_climbController.CanClimbWall()) return;

            Climb();
        }
        public void Fall()
        {
            if (_switchController.IsSwitch(PlayerStateMachine.SwitchEnum.Ladder)) return;
            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Fall;
        }
        public void Land()
        {
            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Land;
        }



        public void Crouch()
        {
            if (!_gravityController.GetIsGrounded()) return;

            if (_stateMachine.StateSwitch == PlayerStateMachine.SwitchEnum.Crouch) _stateMachine.SwitchController.SwitchTo.Idle();
            else _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Crouch;
        }




        public void Climb()
        {
            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Climb;
        }
        public void InAirClimb()
        {
            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.InAirClimb;
        }



        public void Ladder()
        {
            if (_switchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run) || _switchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk) || _switchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle))
            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Ladder;
        }



        public void Swim()
        {
            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Swim;
        }
        public void UnderWater()
        {
            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.UnderWater;
        }

        public void Dash()
        {
            _stateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Dash;
        }
    }
}
