using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickuper : MonoBehaviour
{
    [Header("====Debugs====")]
    [SerializeField] Collider[] _allPickupsDetected;
    [SerializeField] Collider _closestPickup; public Collider ClosestPickup { get { return _closestPickup; } }


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] LayerMask _pickupMask;
    [Range(0, 1)]
    [SerializeField] float _detectionRange;



    private void Update()
    {
        _allPickupsDetected = Physics.OverlapSphere(transform.position, _detectionRange, _pickupMask);
        if (_closestPickup != null)
        {
            _closestPickup.GetComponent<IHighlightable>().UnHighlight();
            _closestPickup = null;
        }
        if (_allPickupsDetected.Length <= 0)
        {
            CanvasController.Instance.HudControllers.Interaction.Pickup.Toggle.Toggle(false);
            return;
        }

        float distanceToClosestOutline = 1000;
        float distanceToCurrentOutline;
        foreach (Collider pickup in _allPickupsDetected)
        {
            distanceToCurrentOutline = Vector3.Distance(pickup.transform.position, transform.position);
            if(distanceToCurrentOutline < distanceToClosestOutline)
            {
                distanceToClosestOutline = distanceToCurrentOutline;
                _closestPickup = pickup;
            }
        }

        if (_closestPickup != null)
        {
            CanvasController.Instance.HudControllers.Interaction.Pickup.Toggle.Toggle(true);
            _closestPickup.GetComponent<IHighlightable>().Highlight();
        }
    }
}
