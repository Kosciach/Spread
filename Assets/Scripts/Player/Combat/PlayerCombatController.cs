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
        if(IsState(CombatStateEnum.Equiped))
        {
            if (choosenWeaponIndex == _equipedWeaponIndex) HideWeapon();
            else SwapWeapon(choosenWeaponIndex);

            return;
        }

        GameObject equipedWeapon = _playerStateMachine.Inventory.Weapons[choosenWeaponIndex];
        WeaponData equipedWeaponData = _playerStateMachine.Inventory.WeaponsData[choosenWeaponIndex];
        if (equipedWeapon == null || equipedWeaponData == null) return;

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
        _playerStateMachine.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.Combat, true, 3);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, false, 3);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, false, 3);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Head, false, 3);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.RangeCombat, true, 3);


        //Set left hand correct transform
        _leftHand.localPosition = _equipedWeaponData.Rest.LeftHand_Position;
        _leftHand.localRotation = Quaternion.Euler(_equipedWeaponData.Rest.LeftHand_Rotation);


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
            _playerStateMachine.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.Combat, false, 3);
            _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, true, 3);
            _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, true, 3);
            _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Head, true, 3);
            _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.RangeCombat, false, 3);


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
        _playerStateMachine.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.Combat, false, 3);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, true, 3);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, true, 3);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Head, true, 3);
        _playerStateMachine.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.RangeCombat, false, 3);


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





    public void SetState(CombatStateEnum state)
    {
        _combatState = state;
    }
    public bool IsState(CombatStateEnum state)
    {
        return _combatState.Equals(state);
    }
}
