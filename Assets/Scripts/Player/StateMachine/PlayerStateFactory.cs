using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class PlayerStateFactory
{
    private PlayerStateMachine _playerStateMachine;

    public PlayerStateFactory(PlayerStateMachine playerStateMachine)
    {
        _playerStateMachine = playerStateMachine;
    }



    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }
    public PlayerBaseState Run()
    {
        return new PlayerRunState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }


    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }
    public PlayerBaseState Fall()
    {
        return new PlayerFallState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }
    public PlayerBaseState Land()
    {
        return new PlayerLandState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }


    public PlayerBaseState Crouch()
    {
        return new PlayerCrouchState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }


    public PlayerBaseState Climb()
    {
        return new PlayerClimbState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }
    public PlayerBaseState InAirClimb()
    {
        return new PlayerInAirClimbState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }


    public PlayerBaseState Ladder()
    {
        return new PlayerLadderState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }

    public PlayerBaseState Swim()
    {
        return new PlayerSwimState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }
    public PlayerBaseState UnderWater()
    {
        return new PlayerUnderWaterState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }

    public PlayerBaseState Dash()
    {
        return new PlayerDashState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }

    public PlayerBaseState AttachmentTable()
    {
        return new PlayerAttachmentTableState(_playerStateMachine, this, MethodBase.GetCurrentMethod().Name);
    }
}
