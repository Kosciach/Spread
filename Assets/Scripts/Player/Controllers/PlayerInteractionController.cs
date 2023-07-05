using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerPickuper _pickuper;



    public void Interaction()
    {
        if (_pickuper.ClosestPickup == null) return;
        _pickuper.ClosestPickup.GetComponent<IPickupable>().Pickup();
    }
}
