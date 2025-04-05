using UnityEngine;
using QuickOutline;
using NaughtyAttributes;
using System;

namespace Spread.Interactions
{
    public abstract class Interactable : MonoBehaviour
    {
        [BoxGroup("Interactable_Ref"), SerializeField] private Outline _outline;
        [BoxGroup("Interactable_Ref"), SerializeField] protected Transform _promptPosRef;
        public Transform PromptWorldRef => _promptPosRef;

        [BoxGroup("Interactable_Data"), SerializeField] public InteractableData InteractableData;

        private float _outlineWidth;
        public Action<Interaction> OnInteraction;


        protected virtual void Awake()
        {
            _outlineWidth = _outline.OutlineWidth;
            _outline.OutlineWidth = 0;
        }

        public virtual void Select(Transform p_player)
        {
            _outline.OutlineWidth = _outlineWidth;
        }

        public virtual void UnSelect()
        {
            _outline.OutlineWidth = 0;
        }
    }

    [System.Serializable]
    public class InteractableData
    {
        public string Name;
        public Interaction[] Interactions;
    }
}
