using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner_Shotgun : BaseBulletSpawner
{
    [Header("====References====")]
    [SerializeField] GameObject _bulletPrefab;

    [Space(20)]
    [Header("====Settings====")]
    [Range(1, 10)] [SerializeField] int _count;
    [Space(5)]
    [Range(0.1f, 10)] [SerializeField] int _offsetX;
    [Range(0.1f, 10)] [SerializeField] int _offsetY;
    [Range(0.1f, 10)] [SerializeField] int _offsetZ;


    public override void SpawnBullet(RangeWeaponData weaponData)
    {
        for(int i=0; i<_count; i++)
        {
            float rotOffsetX = Random.Range(-_offsetX * 10, (_offsetX+ 1) * 10) / 20;
            float rotOffsetY = Random.Range(-_offsetY * 10, (_offsetY+ 1) * 10) / 20;
            float rotOffsetZ = Random.Range(-_offsetZ * 10, (_offsetZ+ 1) * 10) / 20;

            Quaternion rotOffset = Quaternion.Euler(rotOffsetX, rotOffsetY, rotOffsetZ);

            BulletLogic bulletLogic = Instantiate(_bulletPrefab, transform.position, transform.rotation * rotOffset).GetComponent<BulletLogic>();
            bulletLogic.PassData(weaponData.RangeStats.Damage / _count, weaponData.RangeStats.Range, weaponData.RangeStats.PenetrationForce, weaponData.RangeStats.CarredForce / _count);
        }
    }
}
