using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [Header("====Reference====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [Space(5)]
    [SerializeField] Transform _rightHandWeaponHolder;
    [SerializeField] Transform _weaponOrigin;
    [Space(5)]
    [SerializeField] Transform _rightHand;
    [SerializeField] Transform _leftHand;



    [Space(20)]
    [Header("====Debug====")]
    [SerializeField] int _equipedWeaponIndex;
    [SerializeField] GameObject _equipedWeapon;
    [SerializeField] WeaponData _equipedWeaponData;
    [SerializeField] CombatStateEnum _combatState;
    [SerializeField] bool _swap;


    public enum CombatStateEnum
    {
        Unarmed, Equip, Equiped, UnEquip
    }




    public void EquipWeapon(int choosenWeaponIndex)
    {
        if (IsState(CombatStateEnum.Equip) || IsState(CombatStateEnum.UnEquip)) return;

        GameObject equipedWeapon = _playerStateMachine.Inventory.Weapons[choosenWeaponIndex];
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
        _equipedWeapon.GetComponent<RangeWeaponStateMachine>().SwitchController.SwitchTo.Equiped();


        //Prepare hands camera
        _playerStateMachine.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Combat, 5);
        _playerStateMachine.HandsCameraController.RotateController.SetHandsCameraRotation(PlayerHandsCameraRotateController.HandsCameraRotationsEnum.Combat, 5);


        //Toggle layers
        bool enableLayers = _playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle) || _playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk);
        ToggleCombatLayersPreset(enableLayers, 3);


        //Set left hand correct transform
        _leftHand.localPosition = _equipedWeaponData.Rest.LeftHand_Position;
        _leftHand.localRotation = Quaternion.Euler(_equipedWeaponData.Rest.LeftHand_Rotation);


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


            Debug.Log("Move to rest!");
            //Move right hand to rest position
            LeanTween.rotateLocal(_rightHand.gameObject, _equipedWeaponData.AimHip.RightHand_Rotation, 0.3f);
            LeanTween.moveLocal(_rightHand.gameObject, _equipedWeaponData.AimHip.RightHand_Position, 0.5f).setOnComplete(() =>
            {
                SetState(CombatStateEnum.Equiped);
            });
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
