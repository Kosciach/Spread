using IkLayers;
using SimpleMan.CoroutineExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] Transform _throwableHolder;

    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] States _state;                                 public States State { get { return _state; } }
    [SerializeField] ThrowableStateMachine _currentThrowable;


    public enum States
    {
        ReadyToThrow, Hold, StartThrow, Throw, EndThrow
    }

    public void HoldThrow()
    {
        if (_state != States.ReadyToThrow) return;


        if (!_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)
        && !_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)
        && !_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run)
        && !_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump)) return;

        if (_playerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.UnEquip)
        && _playerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equip)) return;


        PlayerInventoryController playerInventory = _playerStateMachine.InventoryControllers.Inventory;
        int notEmptySlotIndex = playerInventory.Throwables.GetFirstNotEmptySlot();
        if (notEmptySlotIndex < 0) return;

        _currentThrowable = Instantiate(playerInventory.Throwables.ThrowableInventorySlots[notEmptySlotIndex].ItemData.ItemPrefab, _throwableHolder).GetComponent<ThrowableStateMachine>();
        _currentThrowable.ChangeState(ThrowableStateMachine.StateLabels.InHand);

        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.BakedWeaponAnimating, true, 0.1f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, true, 0.1f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, true, 0.1f);
        _playerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimator.PlayerAnimatorController.LayersEnum.ThrowBase, true, 0.3f);
        _playerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimator.PlayerAnimatorController.LayersEnum.Throw, true, 0.3f);

        _state = States.Hold;
    }

    public void StartThrow()
    {
        if (_state != States.Hold) return;
        _state = States.StartThrow;

        _playerStateMachine.AnimatingControllers.Animator.SetBool("Throw", true);

        this.Delay(0.3f, () => { Throw(); });
    }
    private  void Throw()
    {
        _state = States.Throw;
        _currentThrowable.ChangeState(ThrowableStateMachine.StateLabels.Thrown);

        Vector3 throwDirection = _playerStateMachine.CameraControllers.Cine.MainCamera.transform.forward + _playerStateMachine.CameraControllers.Cine.MainCamera.transform.up / 10;
        _currentThrowable.Rigidbody.AddForce(throwDirection * _currentThrowable.ThrowableData.ThrowStrenght, ForceMode.Impulse);

        _currentThrowable = null;



        this.Delay(0.833f - 0.3f, () => { EndThrow(); });
    }
    private void EndThrow()
    {
        _playerStateMachine.AnimatingControllers.Animator.SetBool("Throw", false);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.BakedWeaponAnimating, false, 0.3f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 0.3f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 0.3f);
        _playerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimator.PlayerAnimatorController.LayersEnum.ThrowBase, false, 1);
        _playerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimator.PlayerAnimatorController.LayersEnum.Throw, false, 1);

        _state = States.ReadyToThrow;
    }


    public void CancelThrow()
    {
        if (_state != States.Hold) return;
        _state = States.StartThrow;

        Destroy(_currentThrowable.gameObject);

        _state = States.ReadyToThrow;
    }
}
