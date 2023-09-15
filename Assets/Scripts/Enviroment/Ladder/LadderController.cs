using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : LadderCreator, IInteractable, IHighlightable
{
    [Space(20)]
    [Header("====References-LadderController====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    private Outline _outline;

    public LadderCreator.LadderParts Parts { get { return _parts; } }

    private new void Awake()
    {
        base.Awake();
        _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineWidth = 0;
    }


    public void Interact()
    {
        _playerStateMachine.StateControllers.Ladder.OnInteract(_playerStateMachine.transform.position.y >= _ladderTop.position.y, this);
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
