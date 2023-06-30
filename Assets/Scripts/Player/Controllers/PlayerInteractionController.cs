using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerInventoryController _inventory;
    [SerializeField] Transform _playerMainCamera;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] LayerMask _pickupMask;
    [Range(0, 10)]
    [SerializeField] float _pickupDistance;



    public void Interaction()
    {
        RaycastHit pickupHitInfo;

        if (!Physics.Raycast(_playerMainCamera.position, _playerMainCamera.forward, out pickupHitInfo, _pickupDistance, _pickupMask)) return;

        pickupHitInfo.transform.GetComponent<IPickupable>()?.Pickup();
    }
}
