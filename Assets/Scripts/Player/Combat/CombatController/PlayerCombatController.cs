using System.Collections;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using UnityEditor;
using WeaponAnimatorNamespace;

public class PlayerCombatController : MonoBehaviour
{
    [Header("====Reference====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;                public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [SerializeField] PlayerCombat_Equip _equip;                             public PlayerCombat_Equip Equip { get { return _equip; } }
    [SerializeField] PlayerCombat_UnEquip _unEquip;                         public PlayerCombat_UnEquip UnEquip { get { return _unEquip; } }
    [SerializeField] PlayerCombat_Drop _drop;                               public PlayerCombat_Drop Drop { get { return _drop; } }
    [SerializeField] PlayerCombat_TemporaryUnEquip _temporaryUnEquip;       public PlayerCombat_TemporaryUnEquip TemporaryUnEquip { get { return _temporaryUnEquip; } }



    [Space(20)]
    [Header("====Debug====")]
    [SerializeField] CombatStateEnum _combatState;                  public CombatStateEnum CombatState { get { return _combatState; } }
    [SerializeField] WeaponInventorySlot _equipedWeaponSlot;        public WeaponInventorySlot EquipedWeaponSlot { get { return _equipedWeaponSlot; } set { _equipedWeaponSlot = value; } }
    [SerializeField] int _equipedWeaponIndex;                       public int EquipedWeaponIndex { get { return _equipedWeaponIndex; } set { _equipedWeaponIndex = value; } }
    [SerializeField] int _choosenWeaponIndex;                       public int ChoosenWeaponIndex { get { return _choosenWeaponIndex; } set { _choosenWeaponIndex = value; } }
    [SerializeField] bool _swap;                                    public bool Swap { get { return _swap; } set { _swap = value; } }



    public enum CombatStateEnum
    {
        Unarmed, Equip, Equiped, UnEquip, UnarmedTemporary
    }


    public void OnWeaponEquip()
    {
        _equipedWeaponSlot.Weapon.OnWeaponEquip();

        if (_temporaryUnEquip.IsTemporaryUnEquip) return;

        CanvasController.Instance.PanelsControllers.Inventory.SetEquipedWeaponIcon(_equipedWeaponSlot.WeaponData.Icon);
        CanvasController.Instance.PanelsControllers.Inventory.ToggleEquipedWeaponIcon(true);
        WeaponInventorySlot equipedWeaponSlotInInventory = _playerStateMachine.InventoryControllers.Inventory.Weapon.WeaponInventorySlots[_choosenWeaponIndex];
        equipedWeaponSlotInInventory.Equiped = true;
    }
    public void OnWeaponUnEquip()
    {
        if(_equipedWeaponSlot.Weapon != null) _equipedWeaponSlot.Weapon.OnWeaponUnEquip();

        if (_temporaryUnEquip.IsTemporaryUnEquip) return;

        CanvasController.Instance.PanelsControllers.Inventory.ToggleEquipedWeaponIcon(false);
        WeaponInventorySlot equipedWeaponSlotInInventory = _playerStateMachine.InventoryControllers.Inventory.Weapon.WeaponInventorySlots[_choosenWeaponIndex];
        equipedWeaponSlotInInventory.Equiped = false;
    }


    public void SetState(CombatStateEnum state)
    {
        _combatState = state;
    }
    public bool IsState(CombatStateEnum state)
    {
        return _combatState.Equals(state);
    }
}