using IkLayers;
using SimpleMan.CoroutineExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isThrow;


    public void StartThrow()
    {
        if (_isThrow) return;

        if (!_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)
        && !_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)
        && !_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run)
        && !_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump)) return;

        if (_playerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.UnEquip)
        && _playerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equip)) return;


        if (!_playerStateMachine.InventoryControllers.Inventory.Throwables.CheckCanThrow()) return;

        _isThrow = true;
        _playerStateMachine.InventoryControllers.Inventory.Throwables.Throw();
        _isThrow = false;

    }
}
