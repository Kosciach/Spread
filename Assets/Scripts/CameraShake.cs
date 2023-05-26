using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [SerializeField] CinemachineVirtualCamera _cineCamera;
    private CinemachineBasicMultiChannelPerlin _noise;


    private float _amplitude;
    private float _shakeResetSpeed = 10;

    private void Awake()
    {
        CameraShake shake = CameraShake.Instance;
        if (shake != null)
        {
            if (shake != this) Destroy(shake.gameObject);
        }
        else Instance = this;

        _noise = _cineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Update()
    {
        ApplyShake();
        ResetShake();
    }





    private void ApplyShake()
    {
        _noise.m_AmplitudeGain = _amplitude;
    }
    private void ResetShake()
    {
        _amplitude = Mathf.Lerp(_amplitude, 0, _shakeResetSpeed * Time.deltaTime);
    }

    public void Shake(float amplitude, float shakeResetSpeed)
    {
        _amplitude = amplitude;
        _shakeResetSpeed = shakeResetSpeed;
    }
}
