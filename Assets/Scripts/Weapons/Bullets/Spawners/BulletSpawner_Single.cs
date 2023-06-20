using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner_Single : BaseBulletSpawner
{
    [SerializeField] GameObject _bulletPrefab;

    public override void SpawnBullet(RangeWeaponData weaponData)
    {
        BulletLogic bulletLogic = Instantiate(_bulletPrefab, transform.position, transform.rotation).GetComponent<BulletLogic>();
        bulletLogic.PassData(weaponData.RangeStats.Damage, weaponData.RangeStats.Range, weaponData.RangeStats.PenetrationForce, weaponData.RangeStats.CarredForce);
    }
}
