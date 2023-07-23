using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;






    public void ManageThrow()
    {
        if (!_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)
        && !_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)
        && !_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run)
        && !_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump)) return;

        if (_playerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.UnEquip)
        && _playerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equip)) return;


        _playerStateMachine.InventoryControllers.Inventory.Throwables.Throw();
    }
}
