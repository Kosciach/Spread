using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHoldController : WeaponHoldController
{
    private MeleeWeaponData _meleeWeaponData;


    protected override void VirtualAwake()
    {
        _meleeWeaponData = GetComponent<WeaponDataHolder>().WeaponData as MeleeWeaponData;
        _holdMode = HoldModeEnum.Hip;
    }


    public override void ChangeHoldMode(HoldModeEnum mode)
    {
        Debug.Log("Melee weapon cant change to rest :)");
    }



    public override void RestHoldMode(float rotateSpeed, float moveSpeed)
    {
        HipHoldMode(rotateSpeed, moveSpeed);
    }
    public override void HipHoldMode(float rotateSpeed, float moveSpeed)
    {
        CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Lines);
        LeanTween.rotateLocal(_playerCombatController.RightHand.gameObject, _meleeWeaponData.Hip.RightHand_Rotation, rotateSpeed);
        LeanTween.moveLocal(_playerCombatController.RightHand.gameObject, _meleeWeaponData.Hip.RightHand_Position, moveSpeed).setOnComplete(() =>
        {
            _stateMachine.DamageDealingController.enabled = true;
            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);
        });
    }
}
