using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController_Crosshair_Lines : MonoBehaviour
{
    [Header("====Settings====")]
    [SerializeField] Image[] _lines;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 25)]
    [SerializeField] float _distanceFromCenter;



    public void ApplyAccuracy(float accuracyWeight)
    {
        LeanTween.moveLocal(_lines[0].gameObject, new Vector3(0, _distanceFromCenter, 0) * accuracyWeight * 5, 0.1f);
        LeanTween.moveLocal(_lines[1].gameObject, new Vector3(-_distanceFromCenter, 0, 0) * accuracyWeight * 5, 0.1f);
        LeanTween.moveLocal(_lines[2].gameObject, new Vector3(0, -_distanceFromCenter, 0) * accuracyWeight * 5, 0.1f);
        LeanTween.moveLocal(_lines[3].gameObject, new Vector3(_distanceFromCenter, 0, 0) * accuracyWeight * 5, 0.1f);
    }
}
