using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerThrowController;

public class PlayerThrow_Hold : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerThrowController _throwController;



    public void Hold()
    {
        if (!_throwController.IsState(ThrowableStates.ReadyToThrow)) return;
        if (!IsCorrectPlayerState()) return;
        if (!IsCorrectCombatState()) return;
        if (!IsThrowableInSlot()) return;

        _throwController.SetState(ThrowableStates.Hold);



    }



    private bool IsCorrectPlayerState()
    {
        return _throwController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)
                || _throwController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)
                || _throwController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run)
                || _throwController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump);
    }
    private bool IsCorrectCombatState()
    {
        return _throwController.PlayerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equiped)
                || _throwController.PlayerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Unarmed);
    }
    private bool IsThrowableInSlot()
    {
        PlayerInventoryController playerInventory = _throwController.PlayerStateMachine.InventoryControllers.Inventory;
        int notEmptySlotIndex = playerInventory.Throwables.GetFirstNotEmptySlot();
        return notEmptySlotIndex >= 0;
    }
}
