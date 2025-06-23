using UnityEngine;
using QuickOutline;
using System;

namespace Spread.Interactions
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] private Outline _outline;
        [SerializeField] protected Transform _promptPosRef;
        public Transform PromptWorldRef => _promptPosRef;

        [SerializeField] public InteractableData InteractableData;

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
