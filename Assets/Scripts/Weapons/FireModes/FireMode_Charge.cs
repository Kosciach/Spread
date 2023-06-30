using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IkLayers;

public class FireMode_Charge : BaseFireMode
{
    [Header("====References====")]
    [SerializeField] BulletShellEjector _shellEjector;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] int _shootCount;

    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] bool _chargeWithRightHand;
    [Range(0, 1)]
    [SerializeField] int _shootCountOffset;


    protected override void VirtualAwake()
    {
        _fireModeType = WeaponShootingController.FireModeTypeEnum.Charge;
    }


    private void Start()
    {
        _shootCount = 0;

        _inputs.Range.Shoot.performed += ctx =>
        {
            if (!_isInputReady) return;

            if (_weaponShootingController.Shoot()) StartCharge();
        };
    }


    public override void OnReload()
    {
        _shootCount = 0;
        _isInputReady = true;
    }

    private void StartCharge()
    {
        _isInputReady = false;
        _shootCount++;
        StartCoroutine(StartChargeAnimation());
    }
    public void StopCharge()
    {
        _isInputReady = _shootCount < _weaponData.AmmoSettings.MagSize + _shootCountOffset;

        PlayerStateMachine playerStateMachine = _weaponShootingController.StateMachine.PlayerStateMachine;
        playerStateMachine.AnimatingControllers.Fingers.SetUpAllFingers(_weaponData.FingersPreset.Base, 0.2f);
        playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.BakedWeaponAnimating, false, 0.3f);
        playerStateMachine.AnimatingControllers.IkLayers.OnLerpFinish(PlayerIkLayerController.LayerEnum.BakedWeaponAnimating, () =>
        {
            Vector3 pos = _weaponData.InHandPosition;
            Vector3 rot = _weaponData.InHandRotation;
            if (_chargeWithRightHand) playerStateMachine.AnimatingControllers.WeaponHolder.RightHand(transform);
            playerStateMachine.AnimatingControllers.WeaponHolder.SetWeaponInHandTransform(transform, pos, rot);
        });
    }


    private IEnumerator StartChargeAnimation()
    {
        yield return new WaitForSeconds(0.2f);

        PlayerStateMachine playerStateMachine = _weaponShootingController.StateMachine.PlayerStateMachine;

        playerStateMachine.AnimatingControllers.Weapon.BakeTransformer.UpdateBakedTransforms();
        if(_chargeWithRightHand) playerStateMachine.AnimatingControllers.WeaponHolder.LeftHand(transform);
        playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.BakedWeaponAnimating, true, 0.3f);
        playerStateMachine.AnimatingControllers.Animator.SetTrigger("ChargeWeapon", false);
        _weaponShootingController.StateMachine.Animator.SetTrigger("BoltCharge");
    }



    private void EjectShellAtAnimationEvent()
    {
        _shellEjector.EjectShell();
    }
}
