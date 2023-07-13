using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ItemDataHolder : MonoBehaviour, IInteractable, IHighlightable
{
    [Header("====References====")]
    [SerializeField] Item _itemData; public Item ItemData { get { return _itemData; } }


    private Outline _outline;
    private PlayerStateMachine _playerStateMachine;



    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _playerStateMachine = FindObjectOfType<PlayerStateMachine>();
    }



    public void Interact()
    {
        _playerStateMachine.InventoryControllers.Inventory.Item.AddItem(_itemData, this);
    }
    public void Highlight()
    {
        _outline.OutlineWidth = 2;
    }
    public void UnHighlight()
    {
        _outline.OutlineWidth = 0;
    }
}
