using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerThrow;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] PlayerInteractableDetector _interactableDetector;



    public void Interaction()
    {
        if(_playerStateMachine.CombatControllers.Throw.IsState(ThrowableStates.Hold))
        {
            _playerStateMachine.CombatControllers.Throw.Cancel.Cancel();
            return;
        }


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
