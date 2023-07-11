using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractableDetector : MonoBehaviour
{
    [Header("====Debugs====")]
    [SerializeField] Collider[] _allInteractablesDetected;
    [SerializeField] Collider _closestInteractable; public Collider ClosestInteractable { get { return _closestInteractable; } }


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] LayerMask _interactableMask;
    [Range(0, 1)]
    [SerializeField] float _detectionRange;



    private void Update()
    {
        _allInteractablesDetected = Physics.OverlapSphere(transform.position, _detectionRange, _interactableMask);
        if (_closestInteractable != null)
        {
            _closestInteractable.GetComponent<IHighlightable>().UnHighlight();
            _closestInteractable = null;
        }
        if (_allInteractablesDetected.Length <= 0)
        {
            return;
        }

        float distanceToClosestOutline = 1000;
        float distanceToCurrentOutline;
        foreach (Collider interactable in _allInteractablesDetected)
        {
            distanceToCurrentOutline = Vector3.Distance(interactable.transform.position, transform.position);
            if(distanceToCurrentOutline < distanceToClosestOutline)
            {
                distanceToClosestOutline = distanceToCurrentOutline;
                _closestInteractable = interactable;
            }
        }

        if (_closestInteractable != null)
        {
            _closestInteractable.GetComponent<IHighlightable>().Highlight();
        }
    }
}
