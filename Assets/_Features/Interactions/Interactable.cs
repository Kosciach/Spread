using UnityEngine;
using QuickOutline;
using System;
using SaintsField;
using SaintsField.Playa;

namespace Spread.Interactions
{
    public abstract class Interactable : MonoBehaviour
    {
        [LayoutStart("Interactable", ELayout.TitleBox | ELayout.Vertical)]
        [LayoutStart("Interactable/References", ELayout.TitleBox)]
        [SerializeField] private Outline _outline;
        [SerializeField] protected Transform _promptPosRef;
        public Transform PromptWorldRef => _promptPosRef;
        
        [LayoutStart("Interactable/Settings", ELayout.TitleBox)]
        [SerializeField, SaintsRow(inline: true)] public InteractableData InteractableData;

        private float _outlineWidth;
        public Action<Interaction> OnInteraction;

        
        private void Awake()
        {
            _outlineWidth = _outline.OutlineWidth;
            _outline.OutlineWidth = 0;
            
            OnAwake();
        }
        
        public void Select(Transform p_player)
        {
            _outline.OutlineWidth = _outlineWidth;
            
            OnSelect(p_player);
        }
        
        public void UnSelect()
        {
            _outline.OutlineWidth = 0;
            
            OnUnSelect();
        }
        
        protected virtual void OnAwake() { }
        protected virtual void OnSelect(Transform p_playe) { }
        protected virtual void OnUnSelect() { }
    }

    [System.Serializable]
    public class InteractableData
    {
        public string Name;
        public Interaction[] Interactions;
    }
}
