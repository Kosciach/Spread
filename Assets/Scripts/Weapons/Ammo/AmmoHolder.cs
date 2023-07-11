using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class AmmoHolder : MonoBehaviour, IInteractable, IHighlightable
{
    [Header("====Settings====")]
    [SerializeField] Ammo _ammo;
    [SerializeField] int _ammoCount;


    private PlayerStateMachine _playerStateMachine;
    private Outline _outline;

    private void Awake()
    {
        _playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        _outline = GetComponent<Outline>();
    }




    public void Interact()
    {
        _playerStateMachine.InventoryControllers.Inventory.Ammo.AddAmmo(_ammo, _ammoCount);
    }
    public void Highlight()
    {
        _outline.OutlineWidth = 5;
        CanvasController.Instance.HudControllers.Interaction.Pickup.SetWeaponIcon(null);
    }
    public void UnHighlight()
    {
        _outline.OutlineWidth = 0;
    }
}
