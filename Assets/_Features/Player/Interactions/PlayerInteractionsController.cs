using UnityEngine;
using UnityEngine.InputSystem;
using System;
using SaintsField.Playa;

namespace Spread.Player.Interactions
{
    using Input;
    using Spread.Interactions;

    public class PlayerInteractionsController : PlayerControllerBase
    {
        private PlayerInputController _inputController;
        
        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private CapsuleCollider _detector;

        public Interactable CurrentInteractable {  get; private set; }

        public Action OnInteractableChange;
        public Action<Interactable> OnInteract;

        protected override void OnSetup()
        {
            _inputController = _ctx.GetController<PlayerInputController>();
            _inputController.Inputs.Interactions.Use.performed += UseInteraction;
        }

        protected override void OnDispose()
        {
            OnInteractableChange = null;
            OnInteract = null;
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
