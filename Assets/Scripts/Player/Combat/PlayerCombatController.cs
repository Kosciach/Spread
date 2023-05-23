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
        if (!_playerStateMachine.VerticalVelocityController.GravityController.IsGrounded) return;

        if (!IsState(CombatStateEnum.Equiped) && !IsState(CombatStateEnum.Unarmed)) return;




        WeaponStateMachine equipedWeaponNew = _playerStateMachine.Inventory.Weapons[choosenWeaponIndex];
        WeaponData equipedWeaponDataNew = _playerStateMachine.Inventory.WeaponsData[choosenWeaponIndex];
        if (equipedWeaponNew == null || equipedWeaponDataNew == null)
        {
            equipedWeaponNew = _playerStateMachine.Inventory.Fist;
            equipedWeaponDataNew = _playerStateMachine.Inventory.FistData;
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


        CodeExecutionDelayer.Instance.ExecuteAfterDelay(2, () =>
        {
            Debug.Log("equip after 2s");
        });

        //Move right hand to origin
        _weaponOrigin.SetRotation(_equipedWeaponData.WeaponOriginRotation);
        LeanTween.rotate(_rightHand.gameObject, _weaponOrigin.transform.GetChild(0).rotation.eulerAngles, 0.1f);
        LeanTween.move(_rightHand.parent.gameObject, _weaponOrigin.transform.position, 0.1f).setOnComplete(() =>
        {
            //Put weapon in hand
            _equipedWeapon.transform.parent = _rightHandWeaponHolder;
            _equipedWeapon.transform.localPosition = _equipedWeaponData.InHandPosition;
            _equipedWeapon.transform.localRotation = Quaternion.Euler(_equipedWeaponData.InHandRotation);


            //Move right hand to correct position
            _equipedWeapon.HoldController.MoveHandsToCurrentHoldMode(0.3f, 0.5f);
        });
    }




    public void UnEquipWeapon(float unEquipSpeed)
    {
        if (!_playerStateMachine.VerticalVelocityController.GravityController.IsGrounded) return;

        
        if (!IsState(CombatStateEnum.Equiped)) return;

        SetState(CombatStateEnum.UnEquip);


        _playerStateMachine.WeaponAnimator.Bobbing.Toggle(false);
        _playerStateMachine.WeaponAnimator.Sway.Toggle(false);

        _equipedWeapon.DamageDealingController.enabled = false;
        _equipedWeaponController.Aim.ToggleAimBool(false);
        _equipedWeaponController.Block.ToggleBlockBool(false);
        _equipedWeaponController.Run.ToggleRunWeaponLockBool(false);

        CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);

        //Move right hand to origin
        _weaponOrigin.SetRotation(_equipedWeaponData.WeaponOriginRotation);
        LeanTween.rotate(_rightHand.gameObject, _weaponOrigin.transform.GetChild(0).rotation.eulerAngles, 0.3f * unEquipSpeed);
        LeanTween.move(_rightHand.parent.gameObject, _weaponOrigin.transform.position, 0.5f * unEquipSpeed).setOnComplete(() =>
        {
            //Toggle layers
            ToggleCombatLayersPreset(false, true, true, true, false, 3);

            //Disable fingers
            _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 4);
            _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 4);

            //Prepare hands camera
            _playerStateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
            _playerStateMachine.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);


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
        if (_equipedWeaponData.Fists) return;


        //Toggle layers
        ToggleCombatLayersPreset(false, true, true, true, false, 3);




        //Prepare hands camera
        _playerStateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.IdleWalkRun, 5);
        _playerStateMachine.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);




        _playerStateMachine.WeaponAnimator.Bobbing.Toggle(false);
        _playerStateMachine.WeaponAnimator.Sway.Toggle(false);

        _equipedWeapon.DamageDealingController.enabled = false;
        _equipedWeaponController.Aim.ToggleAimBool(false);
        _equipedWeaponController.Block.ToggleBlockBool(false);
        _equipedWeaponController.Run.ToggleRunWeaponLockBool(false);

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

        SetState(CombatStateEnum.Unarmed);
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
        _equipedWeaponController.Aim.ToggleAimBool(false);

        _equipedWeapon = null;
        _equipedWeaponData = null;

    }
    public void RecoverFromTemporaryUnEquip()
    {
        if (_isTemporaryUnEquip) EquipWeapon(_equipedWeaponIndex);
    }


    public void ToggleCombatLayersPreset(bool combatAnim, bool spineLock, bool body, bool head, bool combat, float speed)
    {
        _playerStateMachine.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.Combat, combatAnim, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, spineLock, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, body, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Head, head, speed);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.RangeCombat, combat, speed);
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
