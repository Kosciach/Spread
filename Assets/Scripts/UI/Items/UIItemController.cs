using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


[RequireComponent(typeof(UIItem_Count))]
public class UIItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private UIItem_Count _count;                                public UIItem_Count Count { get { return _count; } }
    private PlayerInventoryController _playerInventory;         public PlayerInventoryController PlayerInventory { get { return _playerInventory; } set { _playerInventory = value; } }
    private Transform _homeParent;                              public Transform HomeParent { get { return _homeParent; } set { _homeParent = value; } }
    private int _indexInInventory;                              public int IndexInInventory { get { return _indexInInventory; } set { _indexInInventory = value; } }
    private bool _isDragged;

    private UIItemControllerInputs _inputs;



    private void Awake()
    {
        _count = GetComponent<UIItem_Count>();

        _inputs = new UIItemControllerInputs();
        _inputs.Disable();
    }
    private void Start()
    {
        _indexInInventory = transform.parent.GetSiblingIndex();
        _inputs.Item.Drop.performed += ctx =>
        {
            if (_isDragged) return;
            _playerInventory.Item.DropItem(_indexInInventory);
        };
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemInventorySlot slotInInventory = _playerInventory.Item.ItemInventorySlots[_indexInInventory];
        if (slotInInventory.Empty) return;

        _inputs.Enable();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _inputs.Disable();
    }




    public void OnBeginDrag(PointerEventData eventData)
    {
        _indexInInventory = transform.parent.GetSiblingIndex();
        ItemInventorySlot slotInInventory = _playerInventory.Item.ItemInventorySlots[_indexInInventory];
        if (slotInInventory.Empty) return;

        _isDragged = true;
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
        _isDragged = false;
    }
}