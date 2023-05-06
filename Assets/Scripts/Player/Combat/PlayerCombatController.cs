using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [Header("====Reference====")]
    [SerializeField] PlayerStateMachine _stateMachine;
    [Space(5)]
    [SerializeField] Transform _rightHandWeaponHolder;



    [Space(20)]
    [Header("====Debug====")]
    [SerializeField] int _equipedWeaponIndex;
    [SerializeField] GameObject _equipedWeapon;
    [SerializeField] WeaponData _equipedWeaponData;
    [SerializeField] CombatStateEnum _combatState;


    public enum CombatStateEnum
    {
        Unarmed, Equip, Equiped, UnEquip
    }







    public void Equip(int choosenWeaponIndex)
    {
        if (!IsState(CombatStateEnum.Unarmed)) return;

        _equipedWeapon = _stateMachine.Inventory.GetWeapon(choosenWeaponIndex);
        _equipedWeaponData = _stateMachine.Inventory.GetWeaponData(choosenWeaponIndex);
        if (_equipedWeapon == null || _equipedWeaponData == null) return;


        _equipedWeaponIndex = choosenWeaponIndex;

        _equipedWeapon.transform.parent = _rightHandWeaponHolder;
        _equipedWeapon.transform.localPosition = _equipedWeaponData.InHandPosition;
        _equipedWeapon.transform.localRotation = Quaternion.Euler(_equipedWeaponData.InHandRotation);

        _stateMachine.HandsCameraController.MoveController.SetHandsCameraPosition(PlayerHandsCameraMoveController.HandsCameraPositionsEnum.Combat, 5);
        _stateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.Combat, 5);

        _stateMachine.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, false, 5);
        _stateMachine.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.RangeCombat, true, 5);

        _equipedWeapon.GetComponent<RangeWeaponStateMachine>().SwitchController.SwitchTo.Equiped();
        SetState(CombatStateEnum.Equiped);
    }
    public void HideWeapon()
    {
        if (IsState(CombatStateEnum.Unarmed)) return;


        _stateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.Base, 5);
        _stateMachine.HandsCameraController.MoveController.SetHandsCameraPosition(PlayerHandsCameraMoveController.HandsCameraPositionsEnum.Idle, 5);

        _stateMachine.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, true, 5);
        _stateMachine.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.RangeCombat, false, 5);

        _stateMachine.Inventory.HolsterWeapon(_equipedWeapon, _equipedWeaponData);

        _equipedWeapon = null;
        _equipedWeaponData = null;

        SetState(CombatStateEnum.Unarmed);
    }
    public void DropWeapon()
    {
        if (!IsState(CombatStateEnum.Equiped)) return;


        _stateMachine.Inventory.DropWeapon(_equipedWeaponIndex);
        _equipedWeapon = null;
        _equipedWeaponData = null;

        _stateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.Base, 5);
        _stateMachine.HandsCameraController.MoveController.SetHandsCameraPosition(PlayerHandsCameraMoveController.HandsCameraPositionsEnum.Idle, 5);

        _stateMachine.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, true, 5);
        _stateMachine.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.RangeCombat, false, 5);

        SetState(CombatStateEnum.Unarmed);
    }







    public void SetState(CombatStateEnum state)
    {
        _combatState = state;
    }
    public bool IsState(CombatStateEnum state)
    {
        return _combatState.Equals(state);
    }
}
