using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquipedWeaponController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCombatController _combatController;


    [Header("====Debugs====")]
    [SerializeField] bool _aim;
    [SerializeField] bool _block;
    [SerializeField] int _aimTypeIndex;
    [SerializeField] AimTypeEnum _aimType;


    private delegate void ADSMethods();
    private ADSMethods[] _asdMethods = new ADSMethods[2];


    public enum AimTypeEnum
    {
        ADS, Left
    }



    private void Awake()
    {
        _asdMethods[0] = AimDisable;    
        _asdMethods[1] = AimEnable;    
    }


    public void ChangeEquipedHoldMode()
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) || _aim) return;
        if (_combatController.EquipedWeaponData.WeaponType == WeaponData.WeaponTypeEnum.Melee) return;
        
        WeaponHoldController equipedModeController = _combatController.EquipedWeapon.EquipedController;
        WeaponHoldController.HoldModeEnum equipedMode = equipedModeController.IsHoldMode(WeaponHoldController.HoldModeEnum.Hip) ? WeaponHoldController.HoldModeEnum.Rest : WeaponHoldController.HoldModeEnum.Hip;

        equipedModeController.ChangeHoldMode(equipedMode);
        equipedModeController.MoveHandsToCurrentHoldMode(0.2f, 0.2f);
    }



    #region Aim
    public void Aim(bool aim)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)) return;

        ToggleAimBool(aim);
        int enableADS = _aim ? 1 : 0;

        _asdMethods[enableADS]();
    }
    public void ToggleAimBool(bool aim)
    {
        _aim = aim;
    }


    private void AimEnable()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = true;
        CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.None);
        MoveHandsToAimTransform();
    }
    private void AimDisable()
    {
        WeaponHoldController equipedModeController = _combatController.EquipedWeapon.EquipedController;
        equipedModeController.MoveHandsToCurrentHoldMode(0.15f, 0.15f);
    }


    public void ResetAimType(int index)
    {
        _aimTypeIndex = index;
        _aimType = (AimTypeEnum)_aimTypeIndex;

    }
    public void ChangeAimType()
    {
        if (!_aim) return;
        if (_combatController.EquipedWeaponData.Aim.Length <= 1) return;


        _aimTypeIndex++;
        _aimTypeIndex = _aimTypeIndex >= _combatController.EquipedWeaponData.Aim.Length ? 0 : _aimTypeIndex;
        _aimType = (AimTypeEnum)_aimTypeIndex;
        _combatController.EquipedWeapon.AimIndexHolder.WeaponAimIndex = _aimTypeIndex;

        MoveHandsToAimTransform();
    }


    private void MoveHandsToAimTransform()
    {
        LeanTween.rotateLocal(_combatController.RightHand.gameObject, _combatController.EquipedWeaponData.Aim[_aimTypeIndex].RightHand_Rotation, 0.15f);
        LeanTween.moveLocal(_combatController.RightHand.gameObject, _combatController.EquipedWeaponData.Aim[_aimTypeIndex].RightHand_Position, 0.15f);
    }
    #endregion



    public void Block()
    {

    }
}
