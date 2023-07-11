using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerInteractableDetector _interactableDetector;



    public void Interaction()
    {
        if (_interactableDetector.ClosestInteractable == null) return;


        IInteractable interactable = _interactableDetector.ClosestInteractable.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact();
            return;
        }


        Debug.Log("No Interaction!");
    }
}
