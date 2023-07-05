using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance { get; private set; }

    [Header("====References====")]
    [SerializeField] HudControllersStruct _hudControllers;          public HudControllersStruct HudControllers { get { return _hudControllers; } }


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




}
