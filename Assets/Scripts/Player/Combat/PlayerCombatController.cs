using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [Header("====Reference====")]
    [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController; public PlayerEquipedWeaponController EquipedWeaponController { get { return _equipedWeaponController; } }
    [Space(5)]
    [SerializeField] Transform _rightHandWeaponHolder;
    [SerializeField] Transform _weaponOrigin;
    [Space(5)]
    [SerializeField] Transform _rightHand; public Transform RightHand { get { return _rightHand; } }
    [SerializeField] Transform _leftHand; public Transform LeftHand { get { return _leftHand; } }



    [Space(20)]
    [Header("====Debug====")]
    [SerializeField] int _equipedWeaponIndex; public int EquipedWeaponIndex { get { return _equipedWeaponIndex; } }
    [SerializeField] WeaponStateMachine _equipedWeapon; public WeaponStateMachine EquipedWeapon { get { return _equipedWeapon; } }
    [SerializeField] WeaponData _equipedWeaponData; public WeaponData EquipedWeaponData { get { return _equipedWeaponData; } }
    [SerializeField] CombatStateEnum _combatState;
    [SerializeField] bool _swap;


    public enum CombatStateEnum
    {
        Unarmed, Equip, Equiped, UnEquip
    }




    public void EquipWeapon(int choosenWeaponIndex)
    {
        if (IsState(CombatStateEnum.Equip) || IsState(CombatStateEnum.UnEquip)) return;

        WeaponStateMachine equipedWeapon = _playerStateMachine.Inventory.Weapons[choosenWeaponIndex];
        WeaponData equipedWeaponData = _playerStateMachine.Inventory.WeaponsData[choosenWeaponIndex];
        if (equipedWeapon == null || equipedWeaponData == null) return;

        if (IsState(CombatStateEnum.Equiped))
        {
            if (choosenWeaponIndex != _equipedWeaponIndex) SwapWeapon(choosenWeaponIndex);

            return;
        }


        _equipedWeaponIndex = choosenWeaponIndex;
        _equipedWeapon = equipedWeapon;
        _equipedWeaponData = equipedWeaponData;


        //Change states
        SetState(CombatStateEnum.Equip);
        _equipedWeapon.SwitchController.SwitchTo.Equiped();


        //Prepare hands camera
        _playerStateMachine.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Combat, 5);
        _playerStateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.Combat, 5);


        //Toggle layers
        bool enableLayers = _playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)
            || _playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)
            || _playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run)
            || _playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Crouch);
        ToggleCombatLayersPreset(enableLayers, 3);


        //Set left hand correct transform
        _leftHand.localPosition = _equipedWeaponData.LeftHand_Position;
        _leftHand.localRotation = Quaternion.Euler(_equipedWeaponData.LeftHand_Rotation);


        //SetupFingers
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersRightHand, true, 4);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersLeftHand, true, 4);
        _playerStateMachine.IkController.Fingers.SetUpAllFingers(_equipedWeaponData.FingersPreset);


        //Move right hand to origin
        LeanTween.rotate(_rightHand.gameObject, _weaponOrigin.rotation.eulerAngles, 0.3f);
        LeanTween.move(_rightHand.gameObject, _weaponOrigin.position, 0.3f).setOnComplete(() =>
        {
            //Put weapon in hand
            _equipedWeapon.transform.parent = _rightHandWeaponHolder;
            _equipedWeapon.transform.localPosition = _equipedWeaponData.InHandPosition;
            _equipedWeapon.transform.localRotation = Quaternion.Euler(_equipedWeaponData.InHandRotation);


            //Move right hand to correct position
            Debug.Log("Move to rest!");
            _equipedWeapon.EquipedController.MoveHandsToCurrentHoldMode(0.3f, 0.5f);
        });
    }




    public void HideWeapon()
    {
        if (!IsState(CombatStateEnum.Equiped)) return;


        //Move right hand to origin
        LeanTween.rotate(_rightHand.gameObject, _weaponOrigin.rotation.eulerAngles, 0.3f);
        LeanTween.move(_rightHand.gameObject, _weaponOrigin.position, 0.5f).setOnComplete(() =>
        {
            //Toggle layers
            ToggleCombatLayersPreset(false, 3);

            //Disable fingers
            _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 4);
            _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 4);

            //Prepare hands camera
            _playerStateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
            _playerStateMachine.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);


            _playerStateMachine.Inventory.HolsterWeapon(_equipedWeapon, _equipedWeaponData);
            _equipedWeapon = null;
            _equipedWeaponData = null;

            _equipedWeaponController.ADS(false);
            SetState(CombatStateEnum.Unarmed);
        });
    }




    public void DropWeapon()
    {
        if (!IsState(CombatStateEnum.Equiped)) return;


        _playerStateMachine.Inventory.DropWeapon(_equipedWeaponIndex);
        _equipedWeapon = null;
        _equipedWeaponData = null;


        //Toggle layers
        ToggleCombatLayersPreset(false, 3);


        //Prepare hands camera
        _playerStateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
        _playerStateMachine.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);


        _equipedWeaponController.ADS(false);
        SetState(CombatStateEnum.Unarmed);
    }




    private void SwapWeapon(int choosenWeaponIndex)
    {
        _swap = true;
        HideWeapon();
        StartCoroutine(ReEquip(choosenWeaponIndex));
    }
    private IEnumerator ReEquip(int choosenWeaponIndex)
    {
        yield return new WaitForSeconds(0.5f);
        EquipWeapon(choosenWeaponIndex);
        _swap = false;
    }




    public void ToggleCombatLayersPreset(bool enable, float speed)
    {
        _playerStateMachine.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.Combat, enable, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, !enable, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, !enable, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Head, !enable, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.RangeCombat, enable, speed);
    }
    public void CheckCombatMovement(bool enable, float speed)
    {
        if(IsState(CombatStateEnum.Equiped))
            ToggleCombatLayersPreset(enable, speed);
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
