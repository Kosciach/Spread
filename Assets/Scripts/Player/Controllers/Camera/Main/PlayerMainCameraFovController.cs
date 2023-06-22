using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainCameraFovController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cineCameraController;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] float _baseFov;


    private CineCameraFovLerp _fovLerp;


    private void Awake()
    {
        _fovLerp = new CineCameraFovLerp(_cineCameraController);
    }



    public void SetFov(float additionalfov, float duration)
    {
        float fov = _baseFov + additionalfov;

        if (_fovLerp.LerpCoroutine != null) StopCoroutine(_fovLerp.LerpCoroutine);

        _fovLerp.LerpCoroutine = _fovLerp.Lerp(fov, duration);
        StartCoroutine(_fovLerp.LerpCoroutine);
    }
}


public class CineCameraFovLerp
{
    private PlayerCineCameraController _cineCameraController;
    private IEnumerator _lerpCoroutine; public IEnumerator LerpCoroutine { get { return _lerpCoroutine; } set { _lerpCoroutine = value; } }

    public CineCameraFovLerp(PlayerCineCameraController cineCameraController)
    {
        _cineCameraController = cineCameraController;
    }




    public IEnumerator Lerp(float endValue, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float time = timeElapsed / duration;

            _cineCameraController.CineCamera.m_Lens.FieldOfView = Mathf.Lerp(_cineCameraController.CineCamera.m_Lens.FieldOfView, endValue, time);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        _cineCameraController.CineCamera.m_Lens.FieldOfView = endValue;
    }
}
