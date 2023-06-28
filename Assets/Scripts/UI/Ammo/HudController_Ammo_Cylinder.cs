using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController_Ammo_Cylinder : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] HudController_Ammo _ammoHudController;
    [SerializeField] TextMeshProUGUI _ammoInMag;
    [SerializeField] CanvasGroup _canvasGroup;


    public void UpdateAmmoInCylinder(int ammoInMag)
    {
        _ammoInMag.text = ammoInMag.ToString();
    }







    public void Toggle(bool enable, float time)
    {
        int targetAlpha = enable ? 1 : 0;

        LeanTween.value(_canvasGroup.alpha, targetAlpha, time).setOnUpdate((float val) => { _canvasGroup.alpha = val; });
    }
}
