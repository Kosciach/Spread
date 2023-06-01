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
    [SerializeField] TogglesSettings _toggles;
    [Space(5)]
    [Range(0, 10)]
    [SerializeField] float _resetRecoilPosSpeed;
    [Range(0, 10)]
    [SerializeField] float _resetRecoilRotSpeed;
    [Range(0, 1)]
    [SerializeField] float _recoilSpeed;


    private int zAngleDirection = 1;
    private int yAngleDirection = -1;



    [System.Serializable]
    private struct TogglesSettings
    {
        public bool MoveBack;
        public bool RotX;
        public bool RotY;
        public bool RotZ;
    }




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
        if (_toggles.MoveBack) MoveBack(recoilSettings);
        if (_toggles.RotX) RotX(recoilSettings);
        if (_toggles.RotY) RotY(recoilSettings);
        if (_toggles.RotZ) RotZ(recoilSettings);
    }

    private void MoveBack(RangeWeaponData.RecoilSettingsStruct recoilSettings)
    {
        LeanTween.value(_recoilVectors.Pos.z, -recoilSettings.BackPush, 0.04f / recoilSettings.Speed).setOnUpdate((float val) =>
        {
            _recoilVectors.Pos.z = val;
        });
    }
    private void RotX(RangeWeaponData.RecoilSettingsStruct recoilSettings)
    {
        LeanTween.value(_recoilVectors.Rot.x, -recoilSettings.RotX, 0.1f / recoilSettings.Speed).setOnUpdate((float val) =>
        {
            _recoilVectors.Rot.x = val;
        });
    }
    private void RotY(RangeWeaponData.RecoilSettingsStruct recoilSettings)
    {
        yAngleDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        float yAngle = recoilSettings.RotY * yAngleDirection;

        LeanTween.value(_recoilVectors.Rot.y, yAngle, 0.1f / recoilSettings.Speed).setEaseInOutBack().setOnUpdate((float val) =>
        {
            _recoilVectors.Rot.y = val;
        });
    }
    private void RotZ(RangeWeaponData.RecoilSettingsStruct recoilSettings)
    {
        zAngleDirection = zAngleDirection == 1 ? -1 : 1;
        float zAngle = recoilSettings.RotZ * zAngleDirection;

        LeanTween.value(_recoilVectors.Rot.z, zAngle, 0.1f / recoilSettings.Speed).setOnUpdate((float val) =>
        {
            _recoilVectors.Rot.z = val;
        }).setOnComplete(() =>
        {
            LeanTween.value(_recoilVectors.Rot.z, -zAngle, 0.1f / recoilSettings.Speed).setOnUpdate((float val) =>
            {
                _recoilVectors.Rot.z = val;
            });
        });
    }
}
