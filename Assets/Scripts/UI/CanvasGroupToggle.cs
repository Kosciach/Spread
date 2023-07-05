using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupToggle
{
    private CanvasGroup _canvasGroup;
    public CanvasGroupToggle(CanvasGroup canvasGroup)
    {
        _canvasGroup = canvasGroup;
    }

    public void Toggle(bool enable)
    {
        int weight = enable ? 1 : 0;
        LeanTween.alphaCanvas(_canvasGroup, weight, 0.1f);
    }
}
