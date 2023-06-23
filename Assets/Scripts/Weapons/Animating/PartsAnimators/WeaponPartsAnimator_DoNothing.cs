using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPartsAnimator_DoNothing : BaseWeaponPartsAnimator
{


    public override void OnShoot(bool isAmmoReadyToBeShoot)
    {
        Debug.Log("Do nothing");
    }
    public override void OnReload()
    {
        Debug.Log("Do nothing");
    }
}