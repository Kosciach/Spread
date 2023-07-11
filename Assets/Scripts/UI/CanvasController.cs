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
    [SerializeField] HudControllersStruct _hudControllers;              public HudControllersStruct HudControllers { get { return _hudControllers; } }
    [SerializeField] AttachmentTableUIController _attachmentTable;      public AttachmentTableUIController AttachmentTable { get { return _attachmentTable; } }


    [System.Serializable]
    public struct HudControllersStruct
    {
        public HudController_Stats Stats;
        public HudController_Crosshair Crosshair;
        public HudController_Ammo Ammo;
        public HudController_Firemodes Firemodes;
        public HudController_Weapon Weapon;
        public HudController_Interaction Interaction;
    }


    private void Awake()
    {
        Instance = this;
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
