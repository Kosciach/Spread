using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBulletSpawner : MonoBehaviour
{
    public abstract void SpawnBullet(RangeWeaponData weaponData);
}
