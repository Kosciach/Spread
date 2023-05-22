using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponRunController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController;
    private PlayerCombatController _combatController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _weaponLock; public bool WeaponLock { get { return _weaponLock; } }



    private delegate void RunHoldMethods();
    private RunHoldMethods[] _runHoldMethods = new RunHoldMethods[2];


    private void Awake()
    {
        _combatController = _equipedWeaponController.CombatController;
        _runHoldMethods[0] = DisableRunHold;
        _runHoldMethods[1] = EnableRunHold;
    }



    public void ToggleRunWeaponLock(bool enable)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) || _equipedWeaponController.Aim.IsAim || _equipedWeaponController.Block.IsBlock) return;

        int index = enable ? 1 : 0;
        ToggleRunWeaponLockBool(enable);

        _runHoldMethods[index]();
    }
    public void ToggleRunWeaponLockBool(bool enable)
    {
        _weaponLock = enable;
    }


    private void EnableRunHold()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = false;
        _combatController.PlayerStateMachine.IkController.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_combatController.EquipedWeaponData, true);


        LeanTween.cancel(_combatController.RightHand.gameObject);
        LeanTween.moveLocal(_combatController.RightHand.parent.gameObject, _combatController.EquipedWeaponData.Run.RightHand_Position, 0.2f);
        LeanTween.rotateLocal(_combatController.RightHand.gameObject, _combatController.EquipedWeaponData.Run.RightHand_Rotation, 0.2f);
    }
    private void DisableRunHold()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = true;
        _combatController.PlayerStateMachine.IkController.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_combatController.EquipedWeaponData, false);


        WeaponHoldController equipedWeaponHoldController = _combatController.EquipedWeapon.HoldController;

        equipedWeaponHoldController.ChangeHoldMode(_combatController.EquipedWeapon.HoldController.HoldMode);
        equipedWeaponHoldController.MoveHandsToCurrentHoldMode(0.2f, 0.2f);
    }
}
