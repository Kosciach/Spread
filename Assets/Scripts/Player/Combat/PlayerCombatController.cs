using System.Collections;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using UnityEditor;

public class PlayerCombatController : MonoBehaviour
{
    [Header("====Reference====")]
    [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [SerializeField] PlayerWeaponWallDetector _weaponWallDetector; public PlayerWeaponWallDetector WeaponWallDetector { get { return _weaponWallDetector;} }
    [Space(5)]
    [SerializeField] Transform _rightHandWeaponHolder;
    [SerializeField] PlayerWeaponOriginController _weaponOrigin;
    [Space(5)]
    [SerializeField] Transform _rightHand; public Transform RightHand { get { return _rightHand; } }




    [Space(20)]
    [Header("====Debug====")]
    [SerializeField] CombatStateEnum _combatState;
    [SerializeField] int _equipedWeaponIndex;
    [SerializeField] int _choosenWeaponIndex;
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
        WeaponMainPositioner mainPositioner = _playerStateMachine.AnimatingControllers.Weapon.MainPositioner;

        if (!_playerStateMachine.MovementControllers.VerticalVelocity.GravityController.IsGrounded) return;

        if (!IsState(CombatStateEnum.Equiped) && !IsState(CombatStateEnum.Unarmed)) return;



        _choosenWeaponIndex = choosenWeaponIndex;
        WeaponStateMachine equipedWeaponNew = _playerStateMachine.InventoryControllers.Inventory.Weapon.Weapons[choosenWeaponIndex];
        WeaponData equipedWeaponDataNew = _playerStateMachine.InventoryControllers.Inventory.Weapon.WeaponsData[choosenWeaponIndex];
        if (equipedWeaponNew == null || equipedWeaponDataNew == null)
        {
            return;
        }



        if (IsState(CombatStateEnum.Equiped))
        {
            if (choosenWeaponIndex != _equipedWeaponIndex) SwapWeapon();
            return;
        }


        SetState(CombatStateEnum.Equip);

        _isTemporaryUnEquip = false;
        _equipedWeaponIndex = choosenWeaponIndex;
        _equipedWeapon = equipedWeaponNew;
        _equipedWeaponData = equipedWeaponDataNew;
        _playerStateMachine.CombatControllers.EquipedWeapon.Aim.ResetAimType(_equipedWeapon.AimIndexHolder.WeaponAimIndex);





        //Change states
        _equipedWeapon.SwitchController.SwitchTo.Equiped();


        //Prepare hands camera
        _playerStateMachine.CameraControllers.Hands.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Combat, 5);
        _playerStateMachine.CameraControllers.Hands.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.Combat, 5);




        //SetupFingers
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, true, 0.1f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, true, 0.1f);
        _playerStateMachine.AnimatingControllers.Fingers.SetUpAllFingers(_equipedWeaponData.FingersPreset.Base);




        //Prepare left hand
        _playerStateMachine.AnimatingControllers.LeftHand.SetPos(_equipedWeaponData.LeftHandTransforms.Base.LeftHand_Position, 10);
        _playerStateMachine.AnimatingControllers.LeftHand.SetRot(_equipedWeaponData.LeftHandTransforms.Base.LeftHand_Rotation, 10);


        //Toggle layers
        //_weaponOrigin.SetRotation(_equipedWeaponData.WeaponOriginRotation);
        ToggleCombatLayersPreset(true, false, false, false, true, 0.4f);
        _playerStateMachine.AnimatingControllers.IkLayers.OnLerpFinish(PlayerIkLayerController.LayerEnum.RangeCombat, () =>
        {
            //Put weapon in hand
            _equipedWeapon.transform.parent = _rightHandWeaponHolder;
            _equipedWeapon.transform.localPosition = _equipedWeaponData.InHandPosition;
            _equipedWeapon.transform.localRotation = Quaternion.Euler(_equipedWeaponData.InHandRotation);


            //Move right hand to correct position
            _equipedWeapon.HoldController.MoveHandsToCurrentHoldMode(1, 1);
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
        _playerStateMachine.CombatControllers.EquipedWeapon.Aim.ToggleAimBool(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Block.ToggleBlockBool(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Run.ToggleRunBool(false);


        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);


        //Move right hand to origin
        _weaponOrigin.SetRotation(_equipedWeaponData.WeaponOriginRotation);
        WeaponMainPositioner mainPositioner = _playerStateMachine.AnimatingControllers.Weapon.MainPositioner;
        mainPositioner.Rotate(_weaponOrigin.transform.GetChild(0).localRotation.eulerAngles, 1);
        mainPositioner.Move(_weaponOrigin.transform.localPosition, 1).SetOnMoveFinish(() => 
        {
            _weaponWallDetector.ToggleCollider(false);

            //Disable fingers
            _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 1);
            _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 1);

            //Prepare hands camera
            _playerStateMachine.CameraControllers.Hands.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
            _playerStateMachine.CameraControllers.Hands.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);

            _equipedWeapon.DamageDealingController.WeaponUnEquiped();

            _playerStateMachine.InventoryControllers.Inventory.Weapon.HolsterWeapon(_equipedWeapon, _equipedWeaponData);
            _equipedWeapon = null;
            _equipedWeaponData = null;


            if (_swap)
            {
                SetState(CombatStateEnum.Unarmed);
                ReEquip(_choosenWeaponIndex);
                return;
            }

            //Toggle layers
            ToggleCombatLayersPreset(false, true, true, true, false, 0.4f);
            _playerStateMachine.AnimatingControllers.IkLayers.OnLerpFinish(PlayerIkLayerController.LayerEnum.RangeCombat, () =>
            {
                SetState(CombatStateEnum.Unarmed);
            });
        });
    }




    public void DropWeapon()
    {
        if (!_playerStateMachine.MovementControllers.VerticalVelocity.GravityController.IsGrounded) return;

        if (!IsState(CombatStateEnum.Equiped)) return;
        if (_equipedWeaponData.Fists) return;


        _weaponWallDetector.ToggleCollider(false);

        //Toggle layers
        ToggleCombatLayersPreset(false, true, true, true, false, 0.2f);




        //Prepare hands camera
        _playerStateMachine.CameraControllers.Hands.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
        _playerStateMachine.CameraControllers.Hands.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);


        //Disable fingers
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 1);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 1);

        //Disable bobbing/sway
        _playerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(false);
        _playerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(false);

        _equipedWeapon.DamageDealingController.Toggle(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Aim.ToggleAimBool(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Block.ToggleBlockBool(false);

        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);

        _equipedWeapon.DamageDealingController.WeaponUnEquiped();

        _playerStateMachine.InventoryControllers.Inventory.Weapon.DropWeapon(_equipedWeaponIndex);
        _equipedWeapon = null;
        _equipedWeaponData = null;

        SetState(CombatStateEnum.Unarmed);
    }






    private void SwapWeapon()
    {
        _swap = true;
        UnEquipWeapon(1);
    }
    private void ReEquip(int choosenWeaponIndex)
    {
        EquipWeapon(choosenWeaponIndex);
        _swap = false;
    }



    public void TemporaryUnEquip()
    {
        if (!IsState(CombatStateEnum.Equiped)) return;

        SetState(CombatStateEnum.Unarmed);
        _isTemporaryUnEquip = true;


        //Toggle layers
        ToggleCombatLayersPreset(false, false, false, false, false, 0.1f);


        //Disable fingers
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 1);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 1);


        //Prepare hands camera
        _playerStateMachine.CameraControllers.Hands.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
        _playerStateMachine.CameraControllers.Hands.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);


        _playerStateMachine.InventoryControllers.Inventory.Weapon.HolsterWeapon(_equipedWeapon, _equipedWeaponData);
        _equipedWeapon.DamageDealingController.Toggle(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Aim.ToggleAimBool(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Block.ToggleBlockBool(false);

        _equipedWeapon.DamageDealingController.WeaponUnEquiped();

        _equipedWeapon = null;
        _equipedWeaponData = null;
    }
    public void RecoverFromTemporaryUnEquip()
    {
        if (_isTemporaryUnEquip) EquipWeapon(_equipedWeaponIndex);
    }


    public void ToggleCombatLayersPreset(bool combatAnim, bool spineLock, bool body, bool head, bool combat, float duration)
    {
        _playerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.Combat, combatAnim, duration);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.SpineLock, spineLock, duration);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Body, body, duration);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Head, head, duration);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.RangeCombat, combat, duration);
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
