using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCineCamera_Horizontal : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cineCameraController;

    private CineCameraRotateHorizontal _cineCameraRotateHorizontal;



    private void Awake()
    {
        _cineCameraRotateHorizontal = new CineCameraRotateHorizontal(_cineCameraController.CineCamera.GetCinemachineComponent<CinemachinePOV>());
    }



    public void RotateToAngle(float angle, float duration)
    {
        if (_cineCameraRotateHorizontal.LerpCoroutine != null) StopCoroutine(_cineCameraRotateHorizontal.LerpCoroutine);

        _cineCameraRotateHorizontal.LerpCoroutine = _cineCameraRotateHorizontal.Lerp(angle, duration);
        StartCoroutine(_cineCameraRotateHorizontal.LerpCoroutine);
    }
    public void ToggleWrap(bool wrap)
    {
        _cineCameraController.CinePOV.m_HorizontalAxis.m_Wrap = wrap;
    }
    public void SetBorderValues(float min, float max)
    {
        _cineCameraController.CinePOV.m_HorizontalAxis.m_MinValue = min;
        _cineCameraController.CinePOV.m_HorizontalAxis.m_MaxValue = max;
    }
}



public class CineCameraRotateHorizontal
{
    private CinemachinePOV _cinePov;
    private IEnumerator _lerpCoroutine; public IEnumerator LerpCoroutine { get { return _lerpCoroutine; } set { _lerpCoroutine = value; } }


    public CineCameraRotateHorizontal(CinemachinePOV cinePov)
    {
        _cinePov = cinePov;
    }


    public IEnumerator Lerp(float endAngle, float duration) 
    {
        float timeElapsed = 0;

        while(timeElapsed < duration)
        {
            float time = timeElapsed / duration;

            _cinePov.m_HorizontalAxis.Value = Mathf.Lerp(_cinePov.m_HorizontalAxis.Value, endAngle, time);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        _cinePov.m_HorizontalAxis.Value = endAngle;
    }
}