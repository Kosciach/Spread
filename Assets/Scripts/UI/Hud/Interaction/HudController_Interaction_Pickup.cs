using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HudController_Interaction_Pickup : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Image _weaponIcon;
    [SerializeField] GameObject _pickupArrowIcon;
    [SerializeField] GameObject _exchangeArrowIcon;


    private CanvasGroupToggle _toggle; public CanvasGroupToggle Toggle { get { return _toggle; } }




    private void Awake()
    {
        _toggle = new CanvasGroupToggle(GetComponent<CanvasGroup>());
    }


    public void SetWeaponIcon(Sprite weaponIcon)
    {
        _weaponIcon.sprite = weaponIcon;
    }
    public void SetArrowIcon(bool isWeaponInventoryFull)
    {
        _pickupArrowIcon.SetActive(!isWeaponInventoryFull);
        _exchangeArrowIcon.SetActive(isWeaponInventoryFull);
    }
}
