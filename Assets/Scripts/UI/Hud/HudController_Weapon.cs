using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HudController_Weapon : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Image _weaponIcon;



    private CanvasGroupToggle _toggle; public CanvasGroupToggle Toggle { get { return _toggle; } }

    private void Awake()
    {
        _toggle = new CanvasGroupToggle(GetComponent<CanvasGroup>());
    }


    public void UpdateIcon(Sprite icon)
    {
        _weaponIcon.sprite = icon;
    }
}
