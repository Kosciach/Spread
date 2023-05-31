using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoilController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] WeaponAnimator.PosRotStruct _recoilVectors; public WeaponAnimator.PosRotStruct RecoilVectors { get { return _recoilVectors; } }
    [Range(0, 1)]
    [SerializeField] int _toggle;



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _resetRecoilPosSpeed;
    [Range(0, 10)]
    [SerializeField] float _resetRecoilRotSpeed;
    [Range(0, 1)]
    [SerializeField] float _recoilSpeed;


    private void Update()
    {
        ResetRecoil();
    }



    private void ResetRecoil()
    {
        _recoilVectors.Pos = Vector3.Lerp(_recoilVectors.Pos, Vector3.zero, _resetRecoilPosSpeed * Time.deltaTime);
        _recoilVectors.Rot = Vector3.Lerp(_recoilVectors.Rot, Vector3.zero, _resetRecoilRotSpeed * Time.deltaTime);
    }


    public void Recoil(RangeWeaponData.RecoilSettingsStruct recoilSettings)
    {
        //Move to the back
        _recoilVectors.Pos = new Vector3(0, 0, -recoilSettings.BackPush);

        //Rotation
        float zAngle = Random.Range(-recoilSettings.RotZ, recoilSettings.RotZ);
        _recoilVectors.Rot = new Vector3(-recoilSettings.RotX, zAngle, zAngle);
    }
}
