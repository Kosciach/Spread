using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PlayerEquipedWeaponController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCombatController _combatController;


    [Header("====Debugs====")]
    [SerializeField] bool _isAim; public bool IsAim { get { return _isAim; } }
    [SerializeField] bool _isBlock; public bool IsBlock { get { return _isBlock; } }
    [SerializeField] int _aimTypeIndex;
    [SerializeField] AimTypeEnum _aimType;


    private delegate void ADSMethods();
    private ADSMethods[] _asdMethods = new ADSMethods[2];

    private delegate void BlockMethods();
    private BlockMethods[] _blockMethods = new BlockMethods[2];

    public enum AimTypeEnum
    {
        ADS, Left
    }



    private void Awake()
    {
        _asdMethods[0] = AimDisable;    
        _asdMethods[1] = AimEnable;

        _blockMethods[0] = BlockDisable;
        _blockMethods[1] = BlockEnable;
    }


    public void ChangeEquipedHoldMode()
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) || _isAim || _isBlock) return;
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

        if (_combatController.EquipedWeaponData.Aim.Length <= 0) return;


        _combatController.PlayerStateMachine.WeaponAnimator.Bobbing.Toggle(!aim);

        ToggleAimBool(aim);
        int enableADS = _isAim ? 1 : 0;

        _asdMethods[enableADS]();
    }
    public void ToggleAimBool(bool aim)
    {
        _isAim = aim;
    }


    private void AimEnable()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = true;
        ToggleBlockBool(false);

        CheckCrosshair();

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
        if (!_isAim) return;
        if (_combatController.EquipedWeaponData.Aim.Length <= 1) return;


        _aimTypeIndex++;
        _aimTypeIndex = _aimTypeIndex >= _combatController.EquipedWeaponData.Aim.Length ? 0 : _aimTypeIndex;
        _aimType = (AimTypeEnum)_aimTypeIndex;
        _combatController.EquipedWeapon.AimIndexHolder.WeaponAimIndex = _aimTypeIndex;

        CheckCrosshair();

        MoveHandsToAimTransform();
    }


    private void MoveHandsToAimTransform()
    {
        LeanTween.cancel(_combatController.RightHand.gameObject);
        LeanTween.rotateLocal(_combatController.RightHand.gameObject, _combatController.EquipedWeaponData.Aim[_aimTypeIndex].RightHand_Rotation, 0.15f);
        LeanTween.moveLocal(_combatController.RightHand.parent.gameObject, _combatController.EquipedWeaponData.Aim[_aimTypeIndex].RightHand_Position, 0.15f);
        _combatController.PlayerStateMachine.IkController.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_combatController.EquipedWeaponData, false);
    }
    #endregion





    private void CheckCrosshair()
    {
        if (_aimType == AimTypeEnum.Left) CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);
        else CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.None);
    }






    #region Block
    public void Block(bool block)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) || _isAim) return;



        _combatController.PlayerStateMachine.WeaponAnimator.Bobbing.Toggle(!block);

        ToggleBlockBool(block);
        int enableBlock = _isBlock ? 1 : 0;

        _blockMethods[enableBlock]();
    }
    public void ToggleBlockBool(bool block)
    {
        _isBlock = block;
    }



    private void BlockEnable()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = false;

        MoveHandsToBlockTransform();
    }
    private void BlockDisable()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = true;

        WeaponHoldController equipedModeController = _combatController.EquipedWeapon.EquipedController;
        equipedModeController.MoveHandsToCurrentHoldMode(0.15f, 0.15f);
    }


    private void MoveHandsToBlockTransform()
    {
        LeanTween.cancel(_combatController.RightHand.gameObject);
        LeanTween.rotateLocal(_combatController.RightHand.gameObject, _combatController.EquipedWeaponData.Block.RightHand_Rotation, 0.15f);
        LeanTween.moveLocal(_combatController.RightHand.parent.gameObject, _combatController.EquipedWeaponData.Block.RightHand_Position, 0.15f);
    }
    #endregion
}
