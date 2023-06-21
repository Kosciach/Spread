using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoHudController_Chamber : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] AmmoHudController _ammoHudController;
    [SerializeField] TextMeshProUGUI _ammoInMag;
    [SerializeField] Image _roundInChamber;
    [SerializeField] CanvasGroup _canvasGroup;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Color[] _roundInChamberColors;



    public void UpdateRoundInChamberColor(bool isRoundInChamber)
    {
        int roundInChamberColorIndex = isRoundInChamber ? 1 : 0;

        _roundInChamber.color = _roundInChamberColors[roundInChamberColorIndex];
    }

    public void UpdateAmmoInMag(int ammoInMag)
    {
        _ammoInMag.text = ammoInMag.ToString();
    }







    public void Toggle(bool enable, float time)
    {
        int targetAlpha = enable ? 1 : 0;

        LeanTween.value(_canvasGroup.alpha, targetAlpha, time).setOnUpdate((float val) => { _canvasGroup.alpha = val; });
    }
}
