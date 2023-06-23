using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoilAnimator : MonoBehaviour
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
    [SerializeField] RecoilSettings _recoilSettings;

    private int zAngleDirection = 1;
    private int yAngleDirection = -1;



    [System.Serializable]
    private struct RecoilSettings
    {
        [Range(0, 10)]
        public float ResetPosSpeed;
        [Range(0, 10)]
        public float ResetRotSpeed;
    }





    private void Update()
    {
        ResetRecoil();
    }



    private void ResetRecoil()
    {
        _recoilVectors.Pos = Vector3.Lerp(_recoilVectors.Pos, Vector3.zero, _recoilSettings.ResetPosSpeed * Time.deltaTime);
        _recoilVectors.Rot = Vector3.Lerp(_recoilVectors.Rot, Vector3.zero, _recoilSettings.ResetRotSpeed * Time.deltaTime);
    }



    public void Recoil(RangeWeaponData.RecoilSettingsStruct recoilSettings)
    {
        MoveBack(recoilSettings);
        RotX(recoilSettings);
        RotY(recoilSettings);
        RotZ(recoilSettings);

        _weaponAnimator.PlayerStateMachine.CameraControllers.Cine.Recoil.Recoil(recoilSettings);
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
