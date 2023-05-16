using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] GameObject[] _crosshairs;
    [SerializeField] GameObject _currentCrosshair;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] CrosshairTypeEnum _crosshairType;



    public enum CrosshairTypeEnum
    {
        None, Dot, Lines
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
}
