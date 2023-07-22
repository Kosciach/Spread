using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


[RequireComponent(typeof(UIItem_Count))]
public class UIItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private UIItem_Count _count;                                public UIItem_Count Count { get { return _count; } }
    private PlayerInventoryController _playerInventory;         public PlayerInventoryController PlayerInventory { get { return _playerInventory; } set { _playerInventory = value; } }
    private Transform _homeParent;                              public Transform HomeParent { get { return _homeParent; } set { _homeParent = value; } }
    private int _indexInInventory;                              public int IndexInInventory { get { return _indexInInventory; } set { _indexInInventory = value; } }
    private UIOrigins _uiOrigin;                                public UIOrigins UIOrigin { get { return _uiOrigin; } }

    private ItemInventorySlot _inventorySlot;

    private bool _isDragged;

    private UIItemWeaponInputs _inputs;



    public enum UIOrigins
    {
        Items, Throwables
    }




    private void Awake()
    {
        _count = GetComponent<UIItem_Count>();

        _inputs = new UIItemWeaponInputs();
        _inputs.Disable();
    }
    private void Start()
    {
        _indexInInventory = transform.parent.GetSiblingIndex();
        _inputs.Both.Drop.performed += ctx =>
        {
            if (_isDragged) return;

            if (transform.parent.GetComponent<UIItemSlotController>()) _playerInventory.Item.DropItem(_indexInInventory);
            else _playerInventory.Throwables.DropThrowable(_indexInInventory);

        };
    }





    public void OnPointerEnter(PointerEventData eventData)
    {
        _inventorySlot = transform.parent.GetComponent<UIItemSlotController>() ? _playerInventory.Item.ItemInventorySlots[_indexInInventory] : _playerInventory.Throwables.ThrowableInventorySlots[_indexInInventory];
        if (_inventorySlot.Empty) return;

        _inputs.Enable();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _inputs.Disable();
    }




    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_inventorySlot.Empty) return;

        _uiOrigin = transform.parent.GetComponent<UIItemSlotController>() ? UIOrigins.Items : UIOrigins.Throwables;

        _isDragged = true;
        _inventorySlot.ItemIcon.raycastTarget = false;
        _homeParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling(); transform.SetAsLastSibling();
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_homeParent);

        _inventorySlot.ItemIcon.raycastTarget = true;
        _isDragged = false;
    }
}