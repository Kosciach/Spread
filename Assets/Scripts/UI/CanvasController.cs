using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance { get; private set; }

    [Header("====References====")]
    [SerializeField] HudControllersStruct _hudControllers; public HudControllersStruct HudControllers { get { return _hudControllers; } }



    [System.Serializable]
    public struct HudControllersStruct
    {
        public CrosshairController Crosshair;
        public AmmoHudController Ammo;
    }


    private void Awake()
    {
        Instance = this;
    }




}
