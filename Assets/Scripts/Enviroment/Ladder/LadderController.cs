using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour, IInteractable, IHighlightable
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] Outline _outline;




    public void Interact()
    {
        _playerStateMachine.StateControllers.Ladder.OnInteract(_playerStateMachine.transform.position.y >= transform.GetChild(0).position.y, this);
    }


    public void Highlight()
    {
        _outline.OutlineWidth = 10;
    }
    public void UnHighlight()
    {
        _outline.OutlineWidth = 0;
    }
}
