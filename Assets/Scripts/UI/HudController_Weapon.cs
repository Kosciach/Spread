using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController_Weapon : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] CanvasController _canvasController;
    [Space(5)]
    [SerializeField] Image _weaponIcon;
    [Space(5)]
    [SerializeField] CanvasGroup _canvasGroup;




    public void UpdateIcon(Sprite icon)
    {
        _weaponIcon.sprite = icon;
    }



    public void Toggle(bool enable, float time)
    {
        int targetAlpha = enable ? 1 : 0;

        LeanTween.value(_canvasGroup.alpha, targetAlpha, time).setOnUpdate((float val) => { _canvasGroup.alpha = val; });
    }
}
