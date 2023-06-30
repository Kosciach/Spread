using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCineCamera_Recoil : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cineCameraController;
    [SerializeField] CineCameraRotationOffset _rotationOffset;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Vector3 _recoil;
    [SerializeField] bool _isShooting;
    [Range(0, 10)]
    [SerializeField] float _resetRecoilTimer = 10;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _recoilResetSpeed;
    [Range(0, 1)]
    [SerializeField] float _weight;



    private void Update()
    {
        Timer();
        ResetRecoil();
        ApplyRecoil();
    }

    private void Timer()
    {
        _resetRecoilTimer -= 40 * Time.deltaTime;
        _resetRecoilTimer = Mathf.Clamp(_resetRecoilTimer, 0, 10);

        _isShooting = _resetRecoilTimer > 0;
    }
    private void ResetRecoil()
    {
        if(!_isShooting) _recoil = Vector3.Lerp(_recoil, Vector3.zero, _recoilResetSpeed * Time.deltaTime);
    }
    private void ApplyRecoil()
    {
        _rotationOffset.m_RecoilOffset = _recoil * _weight;
    }


    public void Recoil(RangeWeaponData.RecoilSettingsStruct recoilSettings)
    {
        _resetRecoilTimer = 10;

        _recoil.x -= recoilSettings.Vertical;
        LeanTween.value(_recoil.y, _recoil.y + Random.Range(-recoilSettings.Horizontal, recoilSettings.Horizontal), 0.1f).setOnUpdate((float val) =>
        {
            _recoil.y = val;
        });
    }
}
