using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController_Crosshair : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] GameObject[] _crosshairs;
    [SerializeField] GameObject _currentCrosshair;
    [Space(5)]
    [SerializeField] CrosshairControllers _crosshairControllers;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] CrosshairTypeEnum _crosshairType;


    [System.Serializable]
    private struct CrosshairControllers
    {
        public HudController_Crosshair_Lines Lines;
        public HudController_Crosshair_Ring Ring;
    }

    public enum CrosshairTypeEnum
    {
        None, Dot, Lines, Ring
    }



    private void Awake()
    {
        SwitchCrosshair(_crosshairType);
    }


    public void SwitchCrosshair(CrosshairTypeEnum crosshairType)
    {
        int index = (int)crosshairType;
        _crosshairType = crosshairType;
        _currentCrosshair = _crosshairs[index];

        foreach (GameObject crosshair in _crosshairs) crosshair.SetActive(false);
        _currentCrosshair.SetActive(true);
    }



    public void ApplyAccuracy(float accuracyWeight)
    {
        if (_crosshairControllers.Lines.gameObject.activeSelf) _crosshairControllers.Lines.ApplyAccuracy(accuracyWeight);
        if (_crosshairControllers.Ring.gameObject.activeSelf) _crosshairControllers.Ring.ApplyAccuracy(accuracyWeight);
    }
}
