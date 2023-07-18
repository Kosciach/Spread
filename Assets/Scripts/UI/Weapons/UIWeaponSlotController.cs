using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIWeaponSlotController : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount <= 0) return;

        bool wasEquipedWeaponMoved = false;
        UIWeaponController droppedUIWeaponController = eventData.pointerDrag.GetComponent<UIWeaponController>();
        UIWeaponController childUIWeaponController = transform.GetChild(0).GetComponent<UIWeaponController>();
        PlayerInventoryController playerInventoryController = childUIWeaponController.PlayerInventory;
        PlayerCombatController playerCombatController = playerInventoryController.StateMachine.CombatControllers.Combat;

        if (droppedUIWeaponController == null) return;
        if (droppedUIWeaponController.HomeParent == null) return;

        wasEquipedWeaponMoved = droppedUIWeaponController.IndexInInventory == playerCombatController.EquipedWeaponIndex
                                || childUIWeaponController.IndexInInventory == playerCombatController.EquipedWeaponIndex;

        childUIWeaponController.HomeParent = droppedUIWeaponController.HomeParent;
        droppedUIWeaponController.HomeParent = transform;
        childUIWeaponController.transform.parent = childUIWeaponController.HomeParent;

        droppedUIWeaponController.IndexInInventory = droppedUIWeaponController.HomeParent.GetSiblingIndex();
        childUIWeaponController.IndexInInventory = childUIWeaponController.HomeParent.GetSiblingIndex();


        WeaponInventorySlot tempWeaponInventorySlot =  playerInventoryController.Weapon.WeaponInventorySlots[droppedUIWeaponController.IndexInInventory];
        playerInventoryController.Weapon.WeaponInventorySlots[droppedUIWeaponController.IndexInInventory] = playerInventoryController.Weapon.WeaponInventorySlots[childUIWeaponController.IndexInInventory];
        playerInventoryController.Weapon.WeaponInventorySlots[childUIWeaponController.IndexInInventory] = tempWeaponInventorySlot;


        if (!playerCombatController.TemporaryUnEquip.IsTemporaryUnEquip) return;
        if (!wasEquipedWeaponMoved) return;

        int newEquipedWeaponSlotIndex = playerInventoryController.Weapon.WeaponInventorySlots.IndexOf(playerCombatController.EquipedWeaponSlot);
        playerCombatController.EquipedWeaponIndex = newEquipedWeaponSlotIndex;
        playerCombatController.ChoosenWeaponIndex = newEquipedWeaponSlotIndex;
    }
}
