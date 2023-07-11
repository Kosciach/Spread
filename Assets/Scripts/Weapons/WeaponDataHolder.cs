using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class WeaponDataHolder : MonoBehaviour, IInteractable, IHighlightable
{
    [Header("====References====")]
    [SerializeField] WeaponData _weaponData; public WeaponData WeaponData { get { return _weaponData; } }



    private PlayerInventoryController _playerInventory;
    private Outline _outline;


    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerInventoryController>();
        _outline = GetComponent<Outline>();
    }



    public void Interact()
    {
        _playerInventory.Weapon.AddWeapon(gameObject.GetComponent<WeaponStateMachine>(), _weaponData);
    }
    public void Highlight()
    {
        _outline.OutlineWidth = 2;
        CanvasController.Instance.HudControllers.Interaction.Pickup.SetWeaponIcon(_weaponData.Icon);
    }
    public void UnHighlight()
    {
        _outline.OutlineWidth = 0;
    }
}
