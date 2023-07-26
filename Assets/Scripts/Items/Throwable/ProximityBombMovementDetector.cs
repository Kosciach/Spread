using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityBombMovementDetector : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] ThrowableController_ProximityBomb _proximityBomb;



    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _proximityBomb.OnDetection();
    }
}
