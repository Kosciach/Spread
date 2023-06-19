using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoHudController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] CanvasController _canvasController; public CanvasController CanvasController { get { return _canvasController; } }
    [SerializeField] AmmoHudsControllersStruct _ammoHudsControllers; public AmmoHudsControllersStruct AmmoHudsControllers { get { return _ammoHudsControllers; } }
    [SerializeField] GameObject[] _ammoHuds;
    [Space(10)]
    [SerializeField] TextMeshProUGUI _ammoInInventory;
    [SerializeField] TextMeshProUGUI _ammoInMag;
    [Space(5)]
    [SerializeField] Image[] _roundImages;
    [SerializeField] Image[] _roundImages_Shadows;
    [Space(5)]
    [SerializeField] CanvasGroup _canvasGroup;



    [System.Serializable]
    public struct AmmoHudsControllersStruct
    {
        public ChamberAmmoHudController Chamber;
    }
    public enum AmmoHudType
    {
        Chamber, Cylinder
    }



    private void Awake()
    {
        _roundImages = new Image[_roundImages_Shadows.Length];
        for(int i=0; i<_roundImages.Length; i++)
            _roundImages[i] = _roundImages_Shadows[i].transform.GetChild(0).GetComponent<Image>();
    }


    public void SwitchAmmoHud(AmmoHudType ammoHudType)
    {
        int index = (int)ammoHudType;

        foreach (GameObject ammoHud in _ammoHuds) ammoHud.SetActive(false);
        _ammoHuds[index].SetActive(true);
    }


    public void UpdateAmmoInInventory(int ammoInInventory)
    {
        _ammoInInventory.text = ammoInInventory.ToString();
    }
    public void UpdateAmmoInMag(int ammoInMag)
    {
        _ammoInMag.text = ammoInMag.ToString();
    }

    public void ChangeRoundIcon(Sprite roundIcon)
    {
        foreach (Image roundImage in _roundImages)
            roundImage.sprite = roundIcon;

        foreach (Image roundImage in _roundImages_Shadows)
            roundImage.sprite = roundIcon;
    }



    public void Toggle(bool enable, float time)
    {
        int targetAlpha = enable ? 1 : 0;

        LeanTween.value(_canvasGroup.alpha, targetAlpha, time).setOnUpdate((float val) => { _canvasGroup.alpha = val; });
    }
}
