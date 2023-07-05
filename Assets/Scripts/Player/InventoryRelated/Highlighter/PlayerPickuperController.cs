using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickuperController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _pickuper;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] LayerMask _pickupMask;
    [SerializeField] LayerMask _ignoreMask;
    [Range(0, 10)]
    [SerializeField] float _pickupDistance;





    private void Update()
    {
        CheckHighlight();
    }


    private void CheckHighlight()
    {
        RaycastHit hitInfo;

        if (!Physics.Raycast(transform.position, transform.forward, out hitInfo, _pickupDistance, ~_ignoreMask)) return;

        _pickuper.position = hitInfo.point;
    }
}