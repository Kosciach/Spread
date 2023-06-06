using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoHudController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] CanvasController _canvasController;
    [Space(10)]
    [SerializeField] TextMeshProUGUI _ammoInMag;
    [SerializeField] TextMeshProUGUI _ammoInInventory;
    [Space(5)]
    [SerializeField] Image[] _roundImages;
    [SerializeField] Image[] _roundImages_Shadows;
    [Space(5)]
    [SerializeField] GameObject[] _fireModePresets;
    [Space(10)]
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

        //foreach (Image roundImage in _roundImages)
        _roundImages[_roundImages.Length-1].color = _roundInChamberColors[roundInChamberColorIndex];
    }
    public void ChangeRoundIcon(Sprite roundIcon)
    {
        foreach(Image roundImage in _roundImages)
            roundImage.sprite = roundIcon;

        foreach (Image roundImage in _roundImages_Shadows)
            roundImage.sprite = roundIcon;
    }

    public void ChangeFireMode(WeaponShootingController.FireModeTypeEnum fireModeType)
    {
        int index = (int)fireModeType;

        foreach (GameObject fireModePreset in _fireModePresets) fireModePreset.SetActive(false);
        _fireModePresets[index].SetActive(true);
    }









    public void Toggle(bool enable, float time)
    {
        int targetAlpha = enable ? 1 : 0;

        LeanTween.value(_canvasGroup.alpha, targetAlpha, time).setOnUpdate((float val) => { _canvasGroup.alpha = val; });
    }
}
