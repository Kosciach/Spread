using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HudController_Ammo_Chamber : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] TextMeshProUGUI _ammoInMag;
    [SerializeField] Image _roundInChamber;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Color[] _roundInChamberColors;



    private CanvasGroupToggle _toggle; public CanvasGroupToggle Toggle { get { return _toggle; } }

    private void Awake()
    {
        _toggle = new CanvasGroupToggle(GetComponent<CanvasGroup>());
    }



    public void UpdateRoundInChamberColor(bool isRoundInChamber)
    {
        int roundInChamberColorIndex = isRoundInChamber ? 1 : 0;

        _roundInChamber.color = _roundInChamberColors[roundInChamberColorIndex];
    }

    public void UpdateAmmoInMag(int ammoInMag)
    {
        _ammoInMag.text = ammoInMag.ToString();
    }
}
