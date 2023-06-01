using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerWeaponInAirController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController;


    private Vector3 _pos;
    private Vector3 _rot;
    public void Jump()
    {
        LeanTween.value(_pos.y, 0.04f, 1).setEaseOutQuart().setOnUpdate((float val) =>
        {
            _pos.y = val;
            _equipedWeaponController.CombatController.PlayerStateMachine.AnimatingControllers.Weapon.HandOffseter.SetPosOffset(_pos, 100);
        });
        LeanTween.value(_rot.x, 15, 1).setEaseOutQuart().setOnUpdate((float val) =>
        {
            _rot.x = val;
            _equipedWeaponController.CombatController.PlayerStateMachine.AnimatingControllers.Weapon.HandOffseter.SetRotOffset(_rot, 100);
        });
    }
    public void Fall()
    {
        LeanTween.value(_pos.y, 0.02f, 0.5f).setEaseInQuart().setOnUpdate((float val) =>
        {
            _pos.y = val;
            _equipedWeaponController.CombatController.PlayerStateMachine.AnimatingControllers.Weapon.HandOffseter.SetPosOffset(_pos, 100);
        });
        LeanTween.value(_rot.x, -15, 0.5f).setEaseInCubic().setOnUpdate((float val) =>
        {
            _rot.x = val;
            _equipedWeaponController.CombatController.PlayerStateMachine.AnimatingControllers.Weapon.HandOffseter.SetRotOffset(_rot, 100);
        });
    }
    public void Land()
    {
        LeanTween.value(_pos.y, 0, 0.3f).setEaseOutBack().setOnUpdate((float val) =>
        {
            _pos.y = val;
            _equipedWeaponController.CombatController.PlayerStateMachine.AnimatingControllers.Weapon.HandOffseter.SetPosOffset(_pos, 100);
        });
        LeanTween.value(_rot.x, 0, 0.5f).setEaseOutBack().setOnUpdate((float val) =>
        {
            _rot.x = val;
            _equipedWeaponController.CombatController.PlayerStateMachine.AnimatingControllers.Weapon.HandOffseter.SetRotOffset(_rot, 100);
        });
    }
}
