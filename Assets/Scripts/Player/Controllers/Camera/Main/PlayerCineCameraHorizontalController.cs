using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCineCameraHorizontalController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cineCameraController;





    public void RotateToAngle(float angle, float time)
    {
        LeanTween.value(_cineCameraController.CinePOV.m_HorizontalAxis.Value, angle, time).setOnUpdate((float val) => { _cineCameraController.CinePOV.m_HorizontalAxis.Value = val; });
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
