using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerThrow;

public class PlayerThrowAnimationsEventReceiver : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;


    public void ThrowAnimStart()
    {
        PlayerThrowController playerThrowController = _playerStateMachine.CombatControllers.Throw;
        playerThrowController.Start.StartThrow();
    }
    public void ThrowAnimPeak()
    {
        PlayerThrowController playerThrowController = _playerStateMachine.CombatControllers.Throw;
        playerThrowController.Throw.Throw();
    }
    public void ThrowAnimEnd()
    {
        PlayerThrowController playerThrowController = _playerStateMachine.CombatControllers.Throw;
        playerThrowController.End.End();
    }

    public void UnEquipAnimEnd()
    {
        PlayerThrowController playerThrowController = _playerStateMachine.CombatControllers.Throw;
        playerThrowController.Cancel.EndCancel();
    }
}
