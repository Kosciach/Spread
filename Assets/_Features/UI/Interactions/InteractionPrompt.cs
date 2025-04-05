using TMPro;
using UnityEngine;

namespace Spread.UI.Interactions
{
    using Player.StateMachine;
    using Spread.Interactions;

    public class InteractionPrompt : MonoBehaviour
    {
        private UnityEngine.Camera _camera;
        private PlayerStateMachine _player;

        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private UIInteraction[] _uiInteractions;

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
            _player = FindFirstObjectByType<PlayerStateMachine>();

            Hide();

            _player.Ctx.InteractionsController.OnInteractableChange += InteractableChange;
        }

        private void Update()
        {
            Interactable interactable = _player.Ctx.InteractionsController.CurrentInteractable;
            transform.position = _camera.WorldToScreenPoint(interactable.PromptWorldRef.position);
        }

        private void Show()
        {
            Interactable interactable = _player.Ctx.InteractionsController.CurrentInteractable;
            transform.position = _camera.WorldToScreenPoint(interactable.PromptWorldRef.position);

            _name.text = interactable.InteractableData.Name;

            int i = 0;
            foreach (var interaction in interactable.InteractableData.Interactions)
            {
                UIInteraction uiInteraction = _uiInteractions[i];
                uiInteraction.Setup(interaction);
                uiInteraction.gameObject.SetActive(true);
                i++;
            }

            gameObject.SetActive(true);
        }

        private void Hide()
        {
            _name.text = "";
            foreach (var uiInteraction in _uiInteractions)
                uiInteraction.gameObject.SetActive(false);

            gameObject.SetActive(false);
        }

        private void InteractableChange()
        {
            if (_player.Ctx.InteractionsController.CurrentInteractable != null)
            {
                Show();
                return;
            }

            Hide();
        }
    }
}
