using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController_Firemodes : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] CanvasController _canvasController;
    [Space(5)]
    [SerializeField] GameObject[] _fireModePresets;
    [Space(10)]
    [SerializeField] CanvasGroup _canvasGroup;



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
