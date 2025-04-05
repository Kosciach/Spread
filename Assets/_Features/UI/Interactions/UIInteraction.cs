using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Spread.UI.Interactions
{
    using Spread.Interactions;

    public class UIInteraction : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _icon;

        private void Awake()
        {
            _name.text = "";
            _icon.sprite = null;
        }

        internal void Setup(Interaction p_interaction)
        {
            _name.text = p_interaction.Name;
            _icon.sprite = p_interaction.InputIcon;
        }
    }
}