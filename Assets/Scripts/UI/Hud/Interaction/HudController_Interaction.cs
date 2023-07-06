using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class HudController_Interaction : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] HudController_Interaction_Pickup _pickup;      public HudController_Interaction_Pickup Pickup { get { return _pickup; } }

    private CanvasGroupToggle _toggle;                              public CanvasGroupToggle Toggle { get { return _toggle; } }




    private void Awake()
    {
        _toggle = new CanvasGroupToggle(GetComponent<CanvasGroup>());
    }
}
