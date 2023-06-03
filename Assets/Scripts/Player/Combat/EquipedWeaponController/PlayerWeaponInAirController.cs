using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerWeaponInAirController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController;


    private SettingsStruct _pos;
    private SettingsStruct _rot;


    private PlayerVerticalVelocityController _verticalVelocity;
    private float _gravityStrength => _verticalVelocity.GravityController.CurrentGravityForce - _verticalVelocity.SlopeController.SlopeAngle;


    private struct SettingsStruct
    {
        public Vector3 Vector;
        public float Speed;
    }




    private void Awake()
    {
        _verticalVelocity = _equipedWeaponController.CombatController.PlayerStateMachine.MovementControllers.VerticalVelocity;
    }
    private void Update()
    {
        _pos.Vector.y = _gravityStrength / 100;
        _rot.Vector.x = _gravityStrength * 4;

        _equipedWeaponController.CombatController.PlayerStateMachine.AnimatingControllers.Weapon.HandOffseter.SetPosOffset(_pos.Vector, _pos.Speed);
        _equipedWeaponController.CombatController.PlayerStateMachine.AnimatingControllers.Weapon.HandOffseter.SetRotOffset(_rot.Vector, _rot.Speed);
    }




    public void Jump()
    {
        _pos.Speed = 5;
        _rot.Speed = 5;
    }
    public void Fall()
    {
        _pos.Speed = 5;
        _rot.Speed = 5;
    }
    public void Land()
    {
        _pos.Speed = 10;
        _rot.Speed = 20;
    }
}
