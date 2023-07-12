using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCineCamera_Vertical : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cineCameraController;

    private CineCameraRotateVertical _cineCameraRotateVertical;



    private void Awake()
    {
        _cineCameraRotateVertical = new CineCameraRotateVertical(_cineCameraController.CineCamera.GetCinemachineComponent<CinemachinePOV>());
    }


    public void RotateToAngle(float angle, float duration)
    {
        if (_cineCameraRotateVertical.LerpCoroutine != null) StopCoroutine(_cineCameraRotateVertical.LerpCoroutine);

        _cineCameraRotateVertical.LerpCoroutine = _cineCameraRotateVertical.Lerp(angle, duration);
        StartCoroutine(_cineCameraRotateVertical.LerpCoroutine);
    }
    public void SetRotateOnFinish(Action toDo)
    {
        _cineCameraRotateVertical.OnFinish = toDo;
    }


    public void ToggleWrap(bool wrap)
    {
        _cineCameraController.CinePOV.m_VerticalAxis.m_Wrap = wrap;
    }
    public void SetBorderValues(float min, float max)
    {
        _cineCameraController.CinePOV.m_VerticalAxis.m_MinValue = min;
        _cineCameraController.CinePOV.m_VerticalAxis.m_MaxValue = max;
    }
}


public class CineCameraRotateVertical
{
    private CinemachinePOV _cinePov;
    public IEnumerator LerpCoroutine;
    public Action OnFinish;

    public CineCameraRotateVertical(CinemachinePOV cinePov)
    {
        _cinePov = cinePov;
    }


    public IEnumerator Lerp(float endAngle, float duration)
    {
        OnFinish = null;

        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float time = timeElapsed / duration;

            _cinePov.m_VerticalAxis.Value = Mathf.Lerp(_cinePov.m_VerticalAxis.Value, endAngle, time);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        _cinePov.m_VerticalAxis.Value = endAngle;

        if (OnFinish != null) OnFinish.Invoke();
    }
}