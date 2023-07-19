using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIWeaponController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private PlayerInventoryController _playerInventory;     public PlayerInventoryController PlayerInventory { get { return _playerInventory; } set { _playerInventory = value; } }
    private Transform _homeParent;                          public Transform HomeParent { get { return _homeParent; } set { _homeParent = value; } }
    private int _indexInInventory;                          public int IndexInInventory { get { return _indexInInventory; } set { _indexInInventory = value; } }

    private Image _raycastReceiver;

    private bool _isDragged;

    private UIItemWeaponInputs _inputs;



    private void Awake()
    {
        _indexInInventory = transform.parent.GetSiblingIndex();
        _raycastReceiver = GetComponent<Image>();

        _inputs = new UIItemWeaponInputs();
        _inputs.Disable();
    }
    private void Start()
    {
        _indexInInventory = transform.parent.GetSiblingIndex();
        _inputs.Both.Drop.performed += ctx =>
        {
            if (_isDragged) return;
            _playerInventory.StateMachine.CombatControllers.Combat.OnWeaponUnEquip();
            CanvasController.Instance.PanelsControllers.Inventory.ToggleEquipedWeaponIcon(false);
            _playerInventory.Weapon.DropWeapon(_indexInInventory);
        };
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        WeaponInventorySlot slotInInventory = _playerInventory.Weapon.WeaponInventorySlots[_indexInInventory];
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
        WeaponInventorySlot slotInInventory = _playerInventory.Weapon.WeaponInventorySlots[_indexInInventory];
        if (slotInInventory.Empty) return;

        _isDragged = true;
        _raycastReceiver.raycastTarget = false;
        _homeParent = transform.parent;
        transform.parent = transform.root;
        transform.SetAsLastSibling();
    }
    public void OnDrag(PointerEventData eventData)
    {
        WeaponInventorySlot slotInInventory = _playerInventory.Weapon.WeaponInventorySlots[_indexInInventory];
        if (slotInInventory.Empty) return;

        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        WeaponInventorySlot slotInInventory = _playerInventory.Weapon.WeaponInventorySlots[_indexInInventory];
        if (slotInInventory.Empty) return;

        transform.parent = _homeParent;
        _raycastReceiver.raycastTarget = true;
        _isDragged = false;
    }
}
