using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFovController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] CinemachineVirtualCamera _cineCamera;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] float _desiredFov;
    [SerializeField] float _currentFov;
    [SerializeField] float _lerpSpeed;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] float _baseFov;

    private void Update()
    {
        LerpFov();
    }


    private void LerpFov()
    {
        _currentFov = Mathf.Lerp(_currentFov, _desiredFov, _lerpSpeed * Time.deltaTime);
        _cineCamera.m_Lens.FieldOfView = _currentFov;
    }


    public void SetFov(float aditionalfov, float lerpSpeed)
    {
        _desiredFov = _baseFov + aditionalfov;
        _desiredFov = Mathf.Clamp(_desiredFov, 75, 110);

        _lerpSpeed = lerpSpeed;
    }
}
