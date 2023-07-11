using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentTableUIController : MonoBehaviour
{

    private CanvasGroupToggle _toggle; public CanvasGroupToggle Toggle { get { return _toggle; } }


    private void Awake()
    {
        _toggle = new CanvasGroupToggle(GetComponent<CanvasGroup>());
    }
}
