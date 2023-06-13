using IkLayers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeFireMode : BaseFireMode
{
    [Header("====Debugs====")]
    [SerializeField] bool _isShootingInput;
    [SerializeField] bool _isCharged = true;


    private BoltActionAmmoController _boltActionAmmoController;


    protected override void VirtualAwake()
    {
        _boltActionAmmoController = GetComponent<BoltActionAmmoController>();
        _fireModeType = WeaponShootingController.FireModeTypeEnum.Charge;
    }
    private void Start()
    {
        _inputs.Range.Shoot.performed += ctx =>
        {
            if (!_isCharged) return;

            _weaponShootingController.Shoot();
            _isCharged = false;

            //Move weapon to lefthand <- to do!



            PlayerStateMachine playerStateMachine = _weaponShootingController.StateMachine.PlayerStateMachine;
            playerStateMachine.AnimatingControllers.Animator.SetBool("ChargeWeapon", true);
            playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.WeaponReload, true, 0.1f);
        };
    }

    public void Charge()
    {
        _boltActionAmmoController.OnCharge();
        _isCharged = true;



        PlayerStateMachine playerStateMachine = _weaponShootingController.StateMachine.PlayerStateMachine;
        playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.WeaponReload, false, 0.5f);

        //Move weapon to right hand <- to do!
    }
}
