using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class HudController_Interaction : MonoBehaviour
{
    private CanvasGroupToggle _toggle; public CanvasGroupToggle Toggle { get { return _toggle; } }

    private void Awake()
    {
        _toggle = new CanvasGroupToggle(GetComponent<CanvasGroup>());
    }
}
