using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractableDetectorPositioner : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _interactableDetector;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] LayerMask _ignoreMask;
    [Range(0, 10)]
    [SerializeField] float _rayDistance;





    private void Update()
    {
        PositionDetector();
    }


    private void PositionDetector()
    {
        RaycastHit hitInfo;

        if (!Physics.Raycast(transform.position, transform.forward, out hitInfo, _rayDistance, ~_ignoreMask)) return;

        _interactableDetector.position = hitInfo.point;
    }
}