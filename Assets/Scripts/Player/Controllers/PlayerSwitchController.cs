using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchController
{
    private PlayerStateMachine _playerStateMachine;
    private SwitchToClass _switchTo; public SwitchToClass SwitchTo { get { return _switchTo; } }


    public PlayerSwitchController(PlayerStateMachine playerStateMachineyer)
    {
        _playerStateMachine = playerStateMachineyer;
        _switchTo = new SwitchToClass(_playerStateMachine, this);
    }




    public bool IsSwitch(PlayerStateMachine.SwitchEnum stateSwitch)
    {
        return _playerStateMachine.StateSwitch == stateSwitch;
    }
}





public class SwitchToClass
{
    private PlayerStateMachine _playerStateMachine;
    private PlayerSwitchController _switchController;
    public SwitchToClass(PlayerStateMachine playerStateMachine, PlayerSwitchController switchController)
    {
        _playerStateMachine = playerStateMachine;
        _switchController = switchController;
    }





    public void Idle()
    {
        _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Idle;
    }
    public void Walk()
    {
        _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Walk;
    }
    public void Run()
    {
        _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Run;
    }



    public void Jump()
    {
        PlayerVerticalVelocityController verticalVelocityController = _playerStateMachine.MovementControllers.VerticalVelocity;

        bool readyToJump = verticalVelocityController.Gravity.IsGrounded && !verticalVelocityController.Jump.CheckAboveObsticle() && verticalVelocityController.Jump.JumpReloaded;
        if (!readyToJump) return;


        //Dash();

        //if (_stateMachine.MovementController.IsDashDirection()) return;

        _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Jump;



        if (!_playerStateMachine.StateControllers.Climb.CanClimbWall()) return;

        Climb();
    }
    public void Fall()
    {
        if (_switchController.IsSwitch(PlayerStateMachine.SwitchEnum.Ladder)) return;
        _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Fall;
    }
    public void Land()
    {
        _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Land;
    }



    public void Crouch()
    {
        if (!_playerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded) return;

        if (_playerStateMachine.StateSwitch == PlayerStateMachine.SwitchEnum.Crouch) _playerStateMachine.SwitchController.SwitchTo.Idle();
        else _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Crouch;
    }




    public void Climb()
    {
        _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Climb;
    }
    public void InAirClimb()
    {
        _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.InAirClimb;
    }



    public void Ladder()
    {
        if (_switchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run)
            || _switchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)
            || _switchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)
            || _switchController.IsSwitch(PlayerStateMachine.SwitchEnum.Crouch))

            _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Ladder;
    }



    public void Swim()
    {
        _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Swim;
    }
    public void UnderWater()
    {
        _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.UnderWater;
    }

    public void Dash()
    {
        _playerStateMachine.StateSwitch = PlayerStateMachine.SwitchEnum.Dash;
    }
}
