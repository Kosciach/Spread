using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HudController_Ammo : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] AmmoHudsControllersStruct _controllers; public AmmoHudsControllersStruct Controllers { get { return _controllers; } }
    [SerializeField] GameObject[] _ammoHuds;
    [Space(10)]
    [SerializeField] TextMeshProUGUI _ammoInInventory;
    [Space(5)]
    [SerializeField] Image[] _roundImages;
    [SerializeField] Image[] _roundImages_Shadows;



    [System.Serializable]
    public struct AmmoHudsControllersStruct
    {
        public HudController_Ammo_Chamber Chamber;
        public HudController_Ammo_Cylinder Cylinder;
        public HudController_Ammo_DoubleBarrel DoubleBarrel;
    }
    public enum AmmoHudType
    {
        Chamber, Cylinder, DoubleBarrel
    }


    private CanvasGroupToggle _toggle; public CanvasGroupToggle Toggle { get { return _toggle; } }


    private void Awake()
    {
        _toggle = new CanvasGroupToggle(GetComponent<CanvasGroup>());

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

    public void ChangeRoundIcon(Sprite roundIcon)
    {
        foreach (Image roundImage in _roundImages)
            roundImage.sprite = roundIcon;

        foreach (Image roundImage in _roundImages_Shadows)
            roundImage.sprite = roundIcon;
    }
}
