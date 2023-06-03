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
    [SerializeField] PlayerWeaponWallDetector _weaponWallDetector; public PlayerWeaponWallDetector WeaponWallDetector { get { return _weaponWallDetector;} }
    [Space(5)]
    [SerializeField] Transform _rightHandWeaponHolder;
    [SerializeField] PlayerWeaponOriginController _weaponOrigin;
    [Space(5)]
    [SerializeField] Transform _rightHand; public Transform RightHand { get { return _rightHand; } }
    [SerializeField] Transform _leftHand; public Transform LeftHand { get { return _leftHand; } }




    [Space(20)]
    [Header("====Debug====")]
    [SerializeField] CombatStateEnum _combatState;
    [SerializeField] int _equipedWeaponIndex;
    [SerializeField] WeaponStateMachine _equipedWeapon; public WeaponStateMachine EquipedWeapon { get { return _equipedWeapon; } }
    [SerializeField] WeaponData _equipedWeaponData; public WeaponData EquipedWeaponData { get { return _equipedWeaponData; } }
    [SerializeField] bool _swap;
    [SerializeField] bool _isTemporaryUnEquip; public bool IsTemporaryUnEquip { get { return _isTemporaryUnEquip; } }


    public enum CombatStateEnum
    {
        Unarmed, Equip, Equiped, UnEquip, UnarmedTemporary
    }




    public void EquipWeapon(int choosenWeaponIndex)
    {
        if (!_playerStateMachine.MovementControllers.VerticalVelocity.GravityController.IsGrounded) return;

        if (!IsState(CombatStateEnum.Equiped) && !IsState(CombatStateEnum.Unarmed)) return;




        WeaponStateMachine equipedWeaponNew = _playerStateMachine.InventoryControllers.Inventory.Weapon.Weapons[choosenWeaponIndex];
        WeaponData equipedWeaponDataNew = _playerStateMachine.InventoryControllers.Inventory.Weapon.WeaponsData[choosenWeaponIndex];
        if (equipedWeaponNew == null || equipedWeaponDataNew == null)
        {
            return;
        }




        if (IsState(CombatStateEnum.Equiped))
        {
            if (choosenWeaponIndex != _equipedWeaponIndex)
            {
                if (equipedWeaponDataNew.Fists && _equipedWeaponData.Fists) return;
                SwapWeapon(choosenWeaponIndex);
            }

            return;
        }


        SetState(CombatStateEnum.Equip);

        _isTemporaryUnEquip = false;
        _equipedWeaponIndex = choosenWeaponIndex;
        _equipedWeapon = equipedWeaponNew;
        _equipedWeaponData = equipedWeaponDataNew;
        _equipedWeaponController.Aim.ResetAimType(_equipedWeapon.AimIndexHolder.WeaponAimIndex);





        //Change states
        _equipedWeapon.SwitchController.SwitchTo.Equiped();


        //Prepare hands camera
        _playerStateMachine.CameraControllers.Hands.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Combat, 5);
        _playerStateMachine.CameraControllers.Hands.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.Combat, 5);


        //Toggle layers
        ToggleCombatLayersPreset(true, false, false, false, true, 3);




        //SetupFingers
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersRightHand, true, 4);
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersLeftHand, true, 4);
        _playerStateMachine.AnimatingControllers.Fingers.SetUpAllFingers(_equipedWeaponData.FingersPreset.Base);




        //Move left hand to position
        _playerStateMachine.AnimatingControllers.LeftHand.SetPos(_equipedWeaponData.LeftHandTransforms.Base.LeftHand_Position, 10);
        _playerStateMachine.AnimatingControllers.LeftHand.SetRot(_equipedWeaponData.LeftHandTransforms.Base.LeftHand_Rotation, 10);


        //Move right hand to origin
        _weaponOrigin.SetRotation(_equipedWeaponData.WeaponOriginRotation);
        _playerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetRot(_weaponOrigin.transform.GetChild(0).localRotation.eulerAngles, 10);
        _playerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetPos(_weaponOrigin.transform.localPosition, 10).CurrentLerpFinished(() =>
        {
            //Put weapon in hand
            _equipedWeapon.transform.parent = _rightHandWeaponHolder;
            _equipedWeapon.transform.localPosition = _equipedWeaponData.InHandPosition;
            _equipedWeapon.transform.localRotation = Quaternion.Euler(_equipedWeaponData.InHandRotation);


            //Move right hand to correct position
            _equipedWeapon.HoldController.MoveHandsToCurrentHoldMode(5, 6);
            _weaponWallDetector.ToggleCollider(true);

            _equipedWeapon.DamageDealingController.WeaponEquiped();
        });
    }




    public void UnEquipWeapon(float unEquipSpeed)
    {
        if (!_playerStateMachine.MovementControllers.VerticalVelocity.GravityController.IsGrounded) return;

        
        if (!IsState(CombatStateEnum.Equiped)) return;

        SetState(CombatStateEnum.UnEquip);


        _playerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(false);
        _playerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(false);

        _equipedWeapon.DamageDealingController.Toggle(false);
        _equipedWeaponController.Aim.ToggleAimBool(false);
        _equipedWeaponController.Block.ToggleBlockBool(false);
        _equipedWeaponController.Run.ToggleRunBool(false);


        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);


        //Move right hand to origin
        _weaponOrigin.SetRotation(_equipedWeaponData.WeaponOriginRotation);
        _playerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetRot(_weaponOrigin.transform.GetChild(0).localRotation.eulerAngles, 10);
        _playerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetPos(_weaponOrigin.transform.localPosition, 10).CurrentLerpFinished(() =>
        {
            _weaponWallDetector.ToggleCollider(false);

            //Toggle layers
            ToggleCombatLayersPreset(false, true, true, true, false, 3);

            //Disable fingers
            _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 4);
            _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 4);

            //Prepare hands camera
            _playerStateMachine.CameraControllers.Hands.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
            _playerStateMachine.CameraControllers.Hands.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);

            _equipedWeapon.DamageDealingController.WeaponUnEquiped();

            _playerStateMachine.InventoryControllers.Inventory.Weapon.HolsterWeapon(_equipedWeapon, _equipedWeaponData);
            _equipedWeapon = null;
            _equipedWeaponData = null;

            SetState(CombatStateEnum.Unarmed);
        });
    }




    public void DropWeapon()
    {
        if (!_playerStateMachine.MovementControllers.VerticalVelocity.GravityController.IsGrounded) return;

        if (!IsState(CombatStateEnum.Equiped)) return;
        if (_equipedWeaponData.Fists) return;


        _weaponWallDetector.ToggleCollider(false);

        //Toggle layers
        ToggleCombatLayersPreset(false, true, true, true, false, 3);




        //Prepare hands camera
        _playerStateMachine.CameraControllers.Hands.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
        _playerStateMachine.CameraControllers.Hands.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);




        _playerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(false);
        _playerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(false);

        _equipedWeapon.DamageDealingController.Toggle(false);
        _equipedWeaponController.Aim.ToggleAimBool(false);
        _equipedWeaponController.Block.ToggleBlockBool(false);
        //_equipedWeaponController.Run.ToggleRunBool(false);

        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);

        _equipedWeapon.DamageDealingController.WeaponUnEquiped();

        _playerStateMachine.InventoryControllers.Inventory.Weapon.DropWeapon(_equipedWeaponIndex);
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

        SetState(CombatStateEnum.Unarmed);
        _isTemporaryUnEquip = true;


        //Toggle layers
        ToggleCombatLayersPreset(false, false, false, false, false, 10);

        //Disable fingers
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 5);
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 5);

        //Prepare hands camera
        _playerStateMachine.CameraControllers.Hands.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
        _playerStateMachine.CameraControllers.Hands.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);


        _playerStateMachine.InventoryControllers.Inventory.Weapon.HolsterWeapon(_equipedWeapon, _equipedWeaponData);
        _equipedWeapon.DamageDealingController.Toggle(false);
        _equipedWeaponController.Aim.ToggleAimBool(false);
        _equipedWeaponController.Block.ToggleBlockBool(false);
        //_equipedWeaponController.Run.ToggleRunBool(false);

        _equipedWeapon.DamageDealingController.WeaponUnEquiped();

        _equipedWeapon = null;
        _equipedWeaponData = null;
    }
    public void RecoverFromTemporaryUnEquip()
    {
        if (_isTemporaryUnEquip) EquipWeapon(_equipedWeaponIndex);
    }


    public void ToggleCombatLayersPreset(bool combatAnim, bool spineLock, bool body, bool head, bool combat, float speed)
    {
        _playerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.Combat, combatAnim, speed);
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, spineLock, speed);
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, body, speed);
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Head, head, speed);
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.RangeCombat, combat, speed);
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
