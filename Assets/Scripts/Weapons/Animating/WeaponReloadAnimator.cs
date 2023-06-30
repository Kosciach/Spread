using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

public class WeaponReloadAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [Space(10)]
    [SerializeField] Transform _rightHandIk;
    [SerializeField] Transform _rightHandIk_Hint;
    [Space(5)]
    [SerializeField] Transform _leftHandIk;
    [SerializeField] Transform _leftHandIk_Hint;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isReloading; public bool IsReloading { get { return _isReloading; } }



    private FingerPreset _fingerPreset;
    private Action _reloadMethod;


    public void Reload(AnimatorOverrideController reloadAnimOveride, FingerPreset fingerPreset, Action reloadMethod)
    {
        if (!_playerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded) return;
        if (_playerStateMachine.CombatControllers.EquipedWeapon.Wall.IsWall) return;

        _fingerPreset = fingerPreset;
        _reloadMethod = reloadMethod;
        SetPreAnimIkTransforms();

        _isReloading = true;
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.BakedWeaponAnimating, true, 0.1f);
        _playerStateMachine.AnimatingControllers.Animator.SetBool("Reload", true);
    }



    public void ReloadFinish()
    {
        _isReloading = false;

        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.BakedWeaponAnimating, false, 5);

        _playerStateMachine.AnimatingControllers.Fingers.SetUpAllFingers(_fingerPreset.Base, 0.01f);

        _reloadMethod();
    }




    private void SetPreAnimIkTransforms()
    {
        _rightHandIk.localPosition = _playerStateMachine.AnimatingControllers.Weapon.MainTransformer.Pos;
        _rightHandIk.localRotation = _playerStateMachine.AnimatingControllers.Weapon.MainTransformer.Rot;

        _leftHandIk.localPosition = _playerStateMachine.AnimatingControllers.LeftHand.CurrentTransformVectors.Pos;
        _leftHandIk.localRotation = Quaternion.Euler(_playerStateMachine.AnimatingControllers.LeftHand.CurrentTransformVectors.Rot);
    }





}
