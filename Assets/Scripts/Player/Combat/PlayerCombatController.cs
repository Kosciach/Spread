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
    [SerializeField] CombatStateEnum _combatState;
    [SerializeField] int _equipedWeaponIndex; public int EquipedWeaponIndex { get { return _equipedWeaponIndex; } }
    [SerializeField] WeaponStateMachine _equipedWeapon; public WeaponStateMachine EquipedWeapon { get { return _equipedWeapon; } }
    [SerializeField] WeaponData _equipedWeaponData; public WeaponData EquipedWeaponData { get { return _equipedWeaponData; } }
    [SerializeField] bool _swap;
    [SerializeField] bool _isTemporaryUnEquip; public bool IsTemporaryUnEquip { get { return _isTemporaryUnEquip; } }


    public enum CombatStateEnum
    {
        Unarmed, Equip, Equiped, UnEquip
    }




    public void EquipWeapon(int choosenWeaponIndex)
    {
        if (!_playerStateMachine.VerticalVelocityController.GravityController.IsGrounded) return;

        if (IsState(CombatStateEnum.Equip) || IsState(CombatStateEnum.UnEquip)) return;

        WeaponStateMachine equipedWeapon = _playerStateMachine.Inventory.Weapons[choosenWeaponIndex];
        WeaponData equipedWeaponData = _playerStateMachine.Inventory.WeaponsData[choosenWeaponIndex];
        if (equipedWeapon == null || equipedWeaponData == null) return;

        if (IsState(CombatStateEnum.Equiped))
        {
            if (choosenWeaponIndex != _equipedWeaponIndex) SwapWeapon(choosenWeaponIndex);

            return;
        }

        _isTemporaryUnEquip = false;
        _equipedWeaponIndex = choosenWeaponIndex;
        _equipedWeapon = equipedWeapon;
        _equipedWeaponData = equipedWeaponData;
        _equipedWeaponController.ResetAimType(_equipedWeapon.AimIndexHolder.WeaponAimIndex);

        //Change states
        SetState(CombatStateEnum.Equip);
        _equipedWeapon.SwitchController.SwitchTo.Equiped();


        //Prepare hands camera
        _playerStateMachine.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Combat, 5);
        _playerStateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.Combat, 5);


        //Toggle layers
        ToggleCombatLayersPreset(true, false, false, false, true, 3);


        //Set left hand correct transform
        _leftHand.localPosition = _equipedWeaponData.LeftHand_Position;
        _leftHand.localRotation = Quaternion.Euler(_equipedWeaponData.LeftHand_Rotation);


        //SetupFingers
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersRightHand, true, 4);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersLeftHand, true, 4);
        _playerStateMachine.IkController.Fingers.SetUpAllFingers(_equipedWeaponData.FingersPreset);


        //Move right hand to origin
        LeanTween.rotate(_rightHand.gameObject, _weaponOrigin.rotation.eulerAngles, 0.3f);
        LeanTween.move(_rightHand.parent.gameObject, _weaponOrigin.position, 0.3f).setOnComplete(() =>
        {
            //Put weapon in hand
            _equipedWeapon.transform.parent = _rightHandWeaponHolder;
            _equipedWeapon.transform.localPosition = _equipedWeaponData.InHandPosition;
            _equipedWeapon.transform.localRotation = Quaternion.Euler(_equipedWeaponData.InHandRotation);


            //Move right hand to correct position
            _equipedWeapon.EquipedController.MoveHandsToCurrentHoldMode(0.3f, 0.5f);
        });
    }




    public void UnEquipWeapon(float unEquipSpeed)
    {
        if (!_playerStateMachine.VerticalVelocityController.GravityController.IsGrounded) return;

        if (!IsState(CombatStateEnum.Equiped)) return;


        //Move right hand to origin
        LeanTween.rotate(_rightHand.gameObject, _weaponOrigin.rotation.eulerAngles, 0.3f * unEquipSpeed);
        LeanTween.move(_rightHand.parent.gameObject, _weaponOrigin.position, 0.5f * unEquipSpeed).setOnComplete(() =>
        {
            //Toggle layers
            ToggleCombatLayersPreset(false, true, true, true, false, 3);

            //Disable fingers
            _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 4);
            _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 4);

            //Prepare hands camera
            _playerStateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
            _playerStateMachine.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);


            _equipedWeapon.DamageDealingController.enabled = false;
            _equipedWeaponController.ToggleAimBool(false);
            CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);

            _playerStateMachine.Inventory.HolsterWeapon(_equipedWeapon, _equipedWeaponData);
            _equipedWeapon = null;
            _equipedWeaponData = null;

            SetState(CombatStateEnum.Unarmed);
        });
    }




    public void DropWeapon()
    {
        if (!_playerStateMachine.VerticalVelocityController.GravityController.IsGrounded) return;

        if (!IsState(CombatStateEnum.Equiped)) return;


        //Toggle layers
        ToggleCombatLayersPreset(false, true, true, true, false, 3);


        //Prepare hands camera
        _playerStateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
        _playerStateMachine.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);

        _equipedWeapon.DamageDealingController.enabled = false;
        _equipedWeaponController.ToggleAimBool(false);
        CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);

        _playerStateMachine.Inventory.DropWeapon(_equipedWeaponIndex);
        _equipedWeapon = null;
        _equipedWeaponData = null;

        SetState(CombatStateEnum.Unarmed);
    }




    private void SwapWeapon(int choosenWeaponIndex)
    {
        _swap = true;
        UnEquipWeapon(1);
        StartCoroutine(ReEquip(choosenWeaponIndex));
    }
    private IEnumerator ReEquip(int choosenWeaponIndex)
    {
        yield return new WaitForSeconds(0.5f);
        EquipWeapon(choosenWeaponIndex);
        _swap = false;
    }



    public void TemporaryUnEquip()
    {
        if (!IsState(CombatStateEnum.Equiped)) return;

        _isTemporaryUnEquip = true;

        //Toggle layers
        ToggleCombatLayersPreset(false, false, false, false, false, 10);

        //Disable fingers
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 5);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 5);

        //Prepare hands camera
        _playerStateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
        _playerStateMachine.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);


        _playerStateMachine.Inventory.HolsterWeapon(_equipedWeapon, _equipedWeaponData);
        _equipedWeapon = null;
        _equipedWeaponData = null;

        _equipedWeaponController.Aim(false);
        SetState(CombatStateEnum.Unarmed);
    }



    public void ToggleCombatLayersPreset(bool combatAnim, bool spineLock, bool body, bool head, bool combat, float speed)
    {
        _playerStateMachine.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.Combat, combatAnim, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, spineLock, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, body, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Head, head, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.RangeCombat, combat, speed);
    }
    public void CheckCombatMovement(bool combatAnim, bool spineLock, bool body, bool head, bool combat, float speed)
    {
        if(IsState(CombatStateEnum.Equiped))
            ToggleCombatLayersPreset(combatAnim, spineLock, body, head, combat, speed);
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
