using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HudController_Ammo_Cylinder : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] TextMeshProUGUI _ammoInMag;



    private CanvasGroupToggle _toggle; public CanvasGroupToggle Toggle { get { return _toggle; } }

    private void Awake()
    {
        _toggle = new CanvasGroupToggle(GetComponent<CanvasGroup>());
    }



    public void UpdateAmmoInCylinder(int ammoInMag)
    {
        _ammoInMag.text = ammoInMag.ToString();
    }

}
