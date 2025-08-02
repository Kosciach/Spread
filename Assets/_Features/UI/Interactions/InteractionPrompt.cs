using TMPro;
using UnityEngine;

namespace Spread.UI.Interactions
{
    using Spread.Interactions;
    using Spread.Player.Interactions;

    public class InteractionPrompt : MonoBehaviour
    {
        private Camera _camera;
        private PlayerInteractionsController _playerInteractionsController;

        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private UIInteraction[] _uiInteractions;

        private void Awake()
        {
            _camera = Camera.main;
            _playerInteractionsController = FindFirstObjectByType<PlayerInteractionsController>();

            Hide();

            _playerInteractionsController.OnInteractableChange += InteractableChange;
        }

        private void Update()
        {
            Interactable interactable = _playerInteractionsController.CurrentInteractable;
            transform.position = _camera.WorldToScreenPoint(interactable.PromptWorldRef.position);
        }

        private void Show()
        {
            Interactable interactable = _playerInteractionsController.CurrentInteractable;
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
            if (_playerInteractionsController.CurrentInteractable != null)
            {
                Show();
                return;
            }

            Hide();
        }
    }
}
