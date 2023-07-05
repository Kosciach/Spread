using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HudController_Ammo_DoubleBarrel : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Image[] _barrelRounds = new Image[2];


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Color[] _barrelRoundsColors;


    private CanvasGroupToggle _toggle; public CanvasGroupToggle Toggle { get { return _toggle; } }

    private void Awake()
    {
        _toggle = new CanvasGroupToggle(GetComponent<CanvasGroup>());
    }



    public void UpdateBarrelRounds(bool barrel0, bool barrel1)
    {
        int colorIndexes0 = barrel0 ? 1 : 0;
        int colorIndexes1 = barrel1 ? 1 : 0;

        _barrelRounds[0].color = _barrelRoundsColors[colorIndexes0];
        _barrelRounds[1].color = _barrelRoundsColors[colorIndexes1];
    }
}
