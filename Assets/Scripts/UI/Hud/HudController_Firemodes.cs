using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class HudController_Firemodes : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] GameObject[] _fireModePresets;



    private CanvasGroupToggle _toggle; public CanvasGroupToggle Toggle { get { return _toggle; } }

    private void Awake()
    {
        _toggle = new CanvasGroupToggle(GetComponent<CanvasGroup>());
    }



    public void ChangeFireMode(WeaponShootingController.FireModeTypeEnum fireModeType)
    {
        int index = (int)fireModeType;

        foreach (GameObject fireModePreset in _fireModePresets) fireModePreset.SetActive(false);
        _fireModePresets[index].SetActive(true);
    }
}
