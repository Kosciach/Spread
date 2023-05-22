using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController;
    private PlayerCombatController _combatController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isBlock; public bool IsBlock { get { return _isBlock; } }


    private delegate void BlockMethods();
    private BlockMethods[] _blockMethods = new BlockMethods[2];




    private void Awake()
    {
        _combatController = _equipedWeaponController.CombatController;
        _blockMethods[0] = BlockDisable;
        _blockMethods[1] = BlockEnable;
    }




    #region Block
    public void Block(bool block)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) || _equipedWeaponController.Aim.IsAim) return;



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
        _equipedWeaponController.Run.ToggleRunWeaponLockBool(false);

        MoveHandsToBlockTransform();
    }
    private void BlockDisable()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = true;

        WeaponHoldController equipedModeController = _combatController.EquipedWeapon.HoldController;
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
