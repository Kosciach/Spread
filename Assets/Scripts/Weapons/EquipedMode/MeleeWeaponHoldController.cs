using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHoldController : WeaponHoldController
{
    private MeleeWeaponData _meleeWeaponData;


    protected override void VirtualAwake()
    {
        _meleeWeaponData = GetComponent<WeaponDataHolder>().WeaponData as MeleeWeaponData;
        ChangeHoldMode(HoldModeEnum.Hip);
    }


    public override void RestHoldMode(float rotateSpeed, float moveSpeed)
    {
        Debug.Log("Cant use rest for Melee!!!");
        return;
    }
    public override void HipHoldMode(float rotateSpeed, float moveSpeed)
    {
        Debug.Log("Hip");
        LeanTween.rotateLocal(_playerCombatController.RightHand.gameObject, _meleeWeaponData.Hip.RightHand_Rotation, rotateSpeed);
        LeanTween.moveLocal(_playerCombatController.RightHand.gameObject, _meleeWeaponData.Hip.RightHand_Position, moveSpeed).setOnComplete(() =>
        {
            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);
        });
    }
}
