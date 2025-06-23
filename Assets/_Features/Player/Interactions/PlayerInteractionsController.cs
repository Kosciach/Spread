using UnityEngine;
using UnityEngine.InputSystem;
using System;
using SaintsField;
using SaintsField.Playa;

namespace Spread.Player.Interactions
{
    using Player.StateMachine;
    using Spread.Interactions;

    public class PlayerInteractionsController : MonoBehaviour
    {
        private PlayerStateMachineContext _ctx;

        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private CapsuleCollider _detector;

        public Interactable CurrentInteractable {  get; private set; }

        public Action OnInteractableChange;
        public Action<Interactable> OnInteract;

        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;

            _ctx.InputController.Inputs.Interactions.Use.performed += UseInteraction;
        }

        private void OnDestroy()
        {
            OnInteractableChange = null;
            OnInteract = null;
        }

        private void Update()
        {
            //CheckInteractables();
        }

        internal void CheckInteractables()
        {
            Vector3 start = _detector.transform.position;
            Vector3 end = _detector.transform.position + _detector.transform.forward * (_detector.height - _detector.radius / 2);
            Collider[] hits = Physics.OverlapCapsule(start, end, _detector.radius);

            float closestDistance = 1000;
            Interactable closestInteractable = null;

            foreach (Collider hit in hits)
            {
                if (!hit.TryGetComponent(out Interactable interactable))
                    continue;

                float distance = Vector3.Distance(start, interactable.PromptWorldRef.position);
                if(distance <= closestDistance)
                {
                    closestInteractable = interactable;
                    closestDistance = distance;
                }
            }

            if(CurrentInteractable != closestInteractable)
            {
                SetInteractable(closestInteractable);
            }
        }

        internal void SetInteractable(Interactable p_interactable)
        {
            CurrentInteractable?.UnSelect();
            CurrentInteractable = p_interactable;
            CurrentInteractable?.Select(transform);
            OnInteractableChange?.Invoke();
        }

        private void UseInteraction(InputAction.CallbackContext p_ctx)
        {
            OnInteract?.Invoke(CurrentInteractable);
        }
    }
}
