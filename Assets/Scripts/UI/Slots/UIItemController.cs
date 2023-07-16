using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private PlayerInventoryController _playerInventory;         public PlayerInventoryController PlayerInventory { get { return _playerInventory; } set { _playerInventory = value; } }
    [SerializeField] Transform _homeParent;                     public Transform HomeParent { get { return _homeParent; } set { _homeParent = value; } }
    [SerializeField] int _indexInInventory;                     public int IndexInInventory { get { return _indexInInventory; } set { _indexInInventory = value; } }


    private void Start()
    {
        _indexInInventory = transform.parent.GetSiblingIndex();
    }




    public void OnBeginDrag(PointerEventData eventData)
    {
        _indexInInventory = transform.parent.GetSiblingIndex();
        ItemInventorySlot slotInInventory = _playerInventory.Item.ItemInventorySlots[_indexInInventory];
        if (slotInInventory.Empty) return;


        slotInInventory.ItemIcon.raycastTarget = false;
        _homeParent = transform.parent;
        transform.parent = transform.root;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        ItemInventorySlot slotInInventory = _playerInventory.Item.ItemInventorySlots[_indexInInventory];
        if (slotInInventory.Empty) return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemInventorySlot slotInInventory = _playerInventory.Item.ItemInventorySlots[_indexInInventory];
        if (slotInInventory.Empty) return;

        transform.parent = _homeParent;
        slotInInventory.ItemIcon.raycastTarget = true;
    }
}