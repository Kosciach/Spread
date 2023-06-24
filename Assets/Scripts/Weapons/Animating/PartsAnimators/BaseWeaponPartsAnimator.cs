using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeaponPartsAnimator : MonoBehaviour
{
    public abstract void OnShoot(bool isAmmoReadyToBeShoot);

    public abstract void OnReload();
}
