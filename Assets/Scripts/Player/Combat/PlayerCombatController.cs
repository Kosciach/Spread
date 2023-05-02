using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [Header("====Reference====")]
    [SerializeField] PlayerInventory _inventory;
    [SerializeField] Transform _rightHandWeaponHolder;



    [Space(20)]
    [Header("====Debug====")]
    [SerializeField] int _equipedWeaponIndex;
    [SerializeField] GameObject _equipedWeapon;
    [SerializeField] WeaponData _equipedWeaponData;
    [SerializeField] CombatStateEnum _combatState;


    public enum CombatStateEnum
    {
        Unarmed, Equip, Equiped, UnEquip
    }







    public void Equip(int choosenWeaponIndex)
    {
        if (!IsState(CombatStateEnum.Unarmed)) return;

        _equipedWeapon = _inventory.GetWeapon(choosenWeaponIndex);
        _equipedWeaponData = _inventory.GetWeaponData(choosenWeaponIndex);
        if (_equipedWeapon == null || _equipedWeaponData == null) return;

        _equipedWeaponIndex = choosenWeaponIndex;

        _equipedWeapon.transform.parent = _rightHandWeaponHolder;
        _equipedWeapon.transform.localPosition = Vector3.zero;
        _equipedWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);

        SetState(CombatStateEnum.Equiped);
    }
    public void HideWeapon()
    {
        if (IsState(CombatStateEnum.Unarmed)) return;

        _equipedWeapon = null;
        _equipedWeaponData = null;
        SetState(CombatStateEnum.Unarmed);
    }
    public void DropWeapon()
    {
        if (!IsState(CombatStateEnum.Equiped)) return;

        _inventory.DropWeapon(_equipedWeaponIndex);
        _equipedWeapon = null;
        _equipedWeaponData = null;

        SetState(CombatStateEnum.Unarmed);
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
