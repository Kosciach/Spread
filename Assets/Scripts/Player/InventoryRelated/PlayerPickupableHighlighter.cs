using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupableHighlighter : MonoBehaviour
{
    private void Highlight(int width, Collider other)
    {
        Outline outline = other.GetComponent<Outline>();
        if (outline == null) return;

        outline.OutlineWidth = width;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("afsfas");
        Highlight(2, other);
    }
    private void OnTriggerExit(Collider other)
    {
        Highlight(0, other);
    }
}
