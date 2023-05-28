using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoHudController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] CanvasController _canvasController;
    [Space(5)]
    [SerializeField] TextMeshProUGUI _ammoInMag;
    [SerializeField] TextMeshProUGUI _ammoInInventory;
    [SerializeField] Image _roundInChamber;
    [Space(5)]
    [SerializeField] CanvasGroup _canvasGroup;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Color[] _roundInChamberColors;



    public void UpdateAmmoInMag(int ammoInMag)
    {
        _ammoInMag.text = ammoInMag.ToString();
    }
    public void UpdateAmmoInInventory(int ammoInInventory)
    {
        _ammoInInventory.text = ammoInInventory.ToString();
    }
    public void UpdateRoundInChamber(bool isRoundInChamber)
    {
        int roundInChamberColorIndex = isRoundInChamber ? 1 : 0;

        _roundInChamber.color = _roundInChamberColors[roundInChamberColorIndex];
    }



    public void Toggle(bool enable, float time)
    {
        int targetAlpha = enable ? 1 : 0;

        LeanTween.value(_canvasGroup.alpha, targetAlpha, time).setOnUpdate((float val) => { _canvasGroup.alpha = val; });
    }
}
