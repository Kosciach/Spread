using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController;
    private PlayerCombatController _combatController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isAim; public bool IsAim { get { return _isAim; } }
    [SerializeField] int _aimTypeIndex;
    [SerializeField] AimTypeEnum _aimType;

    public enum AimTypeEnum
    {
        ADS, Left
    }

    private delegate void AimMethods();
    private AimMethods[] _aimMethods = new AimMethods[2];




    private void Awake()
    {
        _combatController = _equipedWeaponController.CombatController;
        _aimMethods[0] = AimDisable;
        _aimMethods[1] = AimEnable;
    }






    public void Aim(bool aim)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)) return;

        if (_combatController.EquipedWeaponData.Aim.Length <= 0) return;


        _combatController.PlayerStateMachine.WeaponAnimator.Bobbing.Toggle(!aim);

        ToggleAimBool(aim);
        int enableADS = _isAim ? 1 : 0;

        _aimMethods[enableADS]();
    }
    public void ToggleAimBool(bool aim)
    {
        _isAim = aim;
    }


    private void AimEnable()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = true;
        _equipedWeaponController.Block.ToggleBlockBool(false);
        _equipedWeaponController.Run.ToggleRunWeaponLockBool(false);

        CheckCrosshair();

        MoveHandsToAimTransform();
    }
    private void AimDisable()
    {
        WeaponHoldController equipedModeController = _combatController.EquipedWeapon.HoldController;
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
        _combatController.PlayerStateMachine.WeaponAnimator.MainPositioner.SetPos(_combatController.EquipedWeaponData.Aim[_aimTypeIndex].RightHand_Position, 10);
        _combatController.PlayerStateMachine.WeaponAnimator.MainPositioner.SetRot(_combatController.EquipedWeaponData.Aim[_aimTypeIndex].RightHand_Rotation, 10);

        _combatController.PlayerStateMachine.IkController.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_combatController.EquipedWeaponData, false);
    }

    private void CheckCrosshair()
    {
        if (_aimType == AimTypeEnum.Left) CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);
        else CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.None);
    }
}
