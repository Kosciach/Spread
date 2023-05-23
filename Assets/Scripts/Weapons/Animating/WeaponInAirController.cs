using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInAirController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] SettingsStruct _jump;
    [SerializeField] SettingsStruct _fall;
    [SerializeField] SettingsStruct _land;


    [System.Serializable]
    public struct SettingsStruct
    {
        [Header("===Rot===")]
        [Range(-90, 90)]
        public float RotX;
        [Range(0, 10)]
        public float SpeedRot;

        [Space(10)]
        [Header("===Pos===")]
        [Range(-0.1f, 0.1f)]
        public float PosY;
        [Range(0, 10)]
        public float SpeedPos;
    }



    public void Jump()
    {
        _weaponAnimator.HandOffseter.SetRotOffset(new Vector3(_jump.RotX, 0, 0), _jump.SpeedRot);
        _weaponAnimator.HandOffseter.SetPosOffset(new Vector3(0, _jump.PosY, 0), _jump.SpeedPos);
    }
    public void Fall()
    {
        _weaponAnimator.HandOffseter.SetRotOffset(new Vector3(_fall.RotX, 0, 0), _fall.SpeedRot);
        _weaponAnimator.HandOffseter.SetPosOffset(new Vector3(0, _fall.PosY, 0), _fall.SpeedPos);
    }
    public void Land()
    {
        _weaponAnimator.HandOffseter.SetRotOffset(new Vector3(_land.RotX, 0, 0), _land.SpeedRot);
        _weaponAnimator.HandOffseter.SetPosOffset(new Vector3(0, _land.PosY, 0), _land.SpeedPos);
    }
}
