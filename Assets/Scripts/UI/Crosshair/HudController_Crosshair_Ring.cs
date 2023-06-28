using Shapes2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController_Crosshair_Ring : MonoBehaviour
{
    private Shape _ring;
    private RectTransform _ringRectTranform;




    private void Awake()
    {
        _ring = GetComponent<Shape>();
        _ringRectTranform = GetComponent<RectTransform>();
    }

    public void ApplyAccuracy(float accuracyWeight)
    {

    }
}
