using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupableHighlighterController : MonoBehaviour
{
    [Header("====Settings====")]
    [SerializeField] Transform _pickableHighlighter;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] LayerMask _pickupMask;
    [Range(0, 10)]
    [SerializeField] float _pickupDistance;





    private void Update()
    {
        CheckHighlight();
    }


    private void CheckHighlight()
    {
        RaycastHit hitInfo;

        if (!Physics.Raycast(transform.position, transform.forward, out hitInfo, _pickupDistance, _pickupMask)) return;

        _pickableHighlighter.position = hitInfo.point;
    }
}
