using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCineCameraVerticalController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cineCameraController;





    public void RotateToAngle(float angle, float time)
    {
        LeanTween.value(_cineCameraController.CinePOV.m_VerticalAxis.Value, angle, time).setOnUpdate((float val) => { _cineCameraController.CinePOV.m_VerticalAxis.Value = val; });
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
