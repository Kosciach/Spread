using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance { get; private set; }

    [Header("====References====")]
    [SerializeField] Volume _blur;
    [Space(5)]
    [SerializeField] HudControllersStruct _hudControllers;                      public HudControllersStruct HudControllers { get { return _hudControllers; } }
    [SerializeField] PanelsControllersStruct _panelsControllers;              public PanelsControllersStruct PanelsControllers { get { return _panelsControllers; } }
    [SerializeField] AttachmentTableUIController _attachmentTable;              public AttachmentTableUIController AttachmentTable { get { return _attachmentTable; } }


    [System.Serializable]
    public struct HudControllersStruct
    {
        public CanvasGroupToggle MainToggle;
        public HudController_Stats Stats;
        public HudController_Crosshair Crosshair;
        public HudController_Ammo Ammo;
        public HudController_Firemodes Firemodes;
        public HudController_Weapon Weapon;
        public HudController_Interaction Interaction;
    }

    [System.Serializable]
    public struct PanelsControllersStruct
    {
        public CanvasGroupToggle MainToggle;
        public PanelsControllers_Switch Switch;
        public PanelsControllers_Inventory Inventory;
    }



    private void Awake()
    {
        Instance = this;
        _hudControllers.MainToggle = new CanvasGroupToggle(transform.GetChild(0).GetComponent<CanvasGroup>());
        _panelsControllers.MainToggle = new CanvasGroupToggle(transform.GetChild(1).GetComponent<CanvasGroup>());
    }


    public void ToggleBloom(bool enable)
    {
        int weight = enable ? 1 : 0;

        LeanTween.value(_blur.weight, weight, 0.1f).setOnUpdate((float val) => { _blur.weight = val; });
    }
    public void ToggleBloom(bool enable, float transitionDuration)
    {
        int weight = enable ? 1 : 0;

        LeanTween.value(_blur.weight, weight, transitionDuration).setOnUpdate((float val) => { _blur.weight = val; });
    }
}
