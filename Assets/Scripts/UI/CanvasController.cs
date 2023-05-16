using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance { get; private set; }

    [Header("====References====")]
    [SerializeField] CrosshairController _crosshairController; public CrosshairController CrosshairController { get { return _crosshairController; } }



    private void Awake()
    {
        Instance = this;
    }




}
