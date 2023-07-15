using System.Collections;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using UnityEditor;
using WeaponAnimatorNamespace;

public class PlayerCombatController : MonoBehaviour
{
    [Header("====Reference====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;        public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    public PlayerCombat_Equip _equip;                               public PlayerCombat_Equip Equip { get { return _equip; } }
    public PlayerCombat_UnEquip _unEquip;                           public PlayerCombat_UnEquip UnEquip { get { return _unEquip; } }
    public PlayerCombat_Drop _drop;                                 public PlayerCombat_Drop Drop { get { return _drop; } }
    public PlayerCombat_TemporaryUnEquip _temporaryUnEquip;         public PlayerCombat_TemporaryUnEquip TemporaryUnEquip { get { return _temporaryUnEquip; } }



    [Space(20)]
    [Header("====Debug====")]
    [SerializeField] CombatStateEnum _combatState;                  public CombatStateEnum CombatState { get { return _combatState; } }
    [SerializeField] WeaponInventorySlot _equipedWeaponSlot;        public WeaponInventorySlot EquipedWeaponSlot { get { return _equipedWeaponSlot; } set { _equipedWeaponSlot = value; } }
    [SerializeField] int _equipedWeaponIndex;                       public int EquipedWeaponIndex { get { return _equipedWeaponIndex; } set { _equipedWeaponIndex = value; } }
    [SerializeField] int _choosenWeaponIndex;                       public int ChoosenWeaponIndex { get { return _choosenWeaponIndex; } set { _choosenWeaponIndex = value; } }
    [SerializeField] bool _swap;                                    public bool Swap { get { return _swap; } set { _swap = value; } }
    [SerializeField] bool _isTemporaryUnEquip;                      public bool IsTemporaryUnEquip { get { return _isTemporaryUnEquip; } set { _isTemporaryUnEquip = value;  } }



    public enum CombatStateEnum
    {
        Unarmed, Equip, Equiped, UnEquip, UnarmedTemporary
    }



    private void Awake()
    {
        _equip = GetComponent<PlayerCombat_Equip>();
        _unEquip = GetComponent<PlayerCombat_UnEquip>();
        _drop = GetComponent<PlayerCombat_Drop>();
        _temporaryUnEquip = GetComponent<PlayerCombat_TemporaryUnEquip>();
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