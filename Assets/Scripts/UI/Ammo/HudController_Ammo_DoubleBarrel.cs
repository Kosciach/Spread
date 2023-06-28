using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController_Ammo_DoubleBarrel : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] HudController_Ammo _ammoHudController;
    [SerializeField] Image[] _barrelRounds = new Image[2];
    [SerializeField] CanvasGroup _canvasGroup;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Color[] _barrelRoundsColors;


    public void UpdateBarrelRounds(bool barrel0, bool barrel1)
    {
        int colorIndexes0 = barrel0 ? 1 : 0;
        int colorIndexes1 = barrel1 ? 1 : 0;

        _barrelRounds[0].color = _barrelRoundsColors[colorIndexes0];
        _barrelRounds[1].color = _barrelRoundsColors[colorIndexes1];
    }




    public void Toggle(bool enable, float time)
    {
        int targetAlpha = enable ? 1 : 0;

        LeanTween.value(_canvasGroup.alpha, targetAlpha, time).setOnUpdate((float val) => { _canvasGroup.alpha = val; });
    }
}
