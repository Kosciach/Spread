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
    private SubControllersStruct _subControllers;                   public SubControllersStruct SubControllers { get { return _subControllers; } }



    [Space(20)]
    [Header("====Debug====")]
    [SerializeField] CombatStateEnum _combatState;
    [SerializeField] int _equipedWeaponIndex;                       public int EquipedWeaponIndex { get { return _equipedWeaponIndex; } set { _equipedWeaponIndex = value; } }
    [SerializeField] int _choosenWeaponIndex;                       public int ChoosenWeaponIndex { get { return _choosenWeaponIndex; } set { _choosenWeaponIndex = value; } }
    [SerializeField] WeaponInventorySlot _equipedWeaponSlot;        public WeaponInventorySlot EquipedWeaponSlot { get { return _equipedWeaponSlot; } set { _equipedWeaponSlot = value;  } }
    [SerializeField] bool _swap;                                    public bool Swap { get { return _swap; } set { _swap = value; } }
    [SerializeField] bool _isTemporaryUnEquip;                      public bool IsTemporaryUnEquip { get { return _isTemporaryUnEquip; } set { _isTemporaryUnEquip = value;  } }



    [System.Serializable]
    public struct SubControllersStruct
    {
        public PlayerCombat_Equip Equip;
        public PlayerCombat_UnEquip UnEquip;
        public PlayerCombat_Drop Drop;
        public PlayerCombat_TemporaryUnEquip TemporaryUnEquip;
    }

    public enum CombatStateEnum
    {
        Unarmed, Equip, Equiped, UnEquip, UnarmedTemporary
    }



    private void Awake()
    {
        _subControllers.Equip = GetComponent<PlayerCombat_Equip>();
        _subControllers.UnEquip = GetComponent<PlayerCombat_UnEquip>();
        _subControllers.Drop = GetComponent<PlayerCombat_Drop>();
        _subControllers.TemporaryUnEquip = GetComponent<PlayerCombat_TemporaryUnEquip>();
    }

    public void EquipWeapon(int choosenWeaponIndex)
    {
        WeaponAnimator_MainTransformer mainPositioner = _playerStateMachine.AnimatingControllers.Weapon.MainTransformer;

        if (!_playerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded) return;

        if (!IsState(CombatStateEnum.Equiped) && !IsState(CombatStateEnum.Unarmed)) return;



        _choosenWeaponIndex = choosenWeaponIndex;
        WeaponInventorySlot equipedWeaponSlotNew = _playerStateMachine.InventoryControllers.Inventory.Weapon.WeaponInventorySlots[choosenWeaponIndex];
        if (equipedWeaponSlotNew.Weapon == null || equipedWeaponSlotNew.WeaponData == null)
        {
            return;
        }



        if (IsState(CombatStateEnum.Equiped))
        {
            if (choosenWeaponIndex != _equipedWeaponIndex) SwapWeapon();
            return;
        }


        SetState(CombatStateEnum.Equip);

        _isTemporaryUnEquip = false;
        _equipedWeaponIndex = choosenWeaponIndex;
        _equipedWeaponSlot = equipedWeaponSlotNew;
        _playerStateMachine.CombatControllers.EquipedWeapon.Aim.LoadAimTypeFromWeapon(_equipedWeaponSlot.Weapon);

        _playerStateMachine.AnimatingControllers.Animator.OverrideAnimator(_equipedWeaponSlot.Weapon.AnimatorOverride);

        //Change states
        _equipedWeaponSlot.Weapon.SwitchController.SwitchTo.Equiped();


        //Prepare hands camera
        _playerStateMachine.CameraControllers.Hands.Move.SetCameraPosition(PlayerHandsCamera_Move.CameraPositionsEnum.Combat, 5);
        _playerStateMachine.CameraControllers.Hands.Rotate.SetHandsCameraRotation(PlayerHandsCamera_Rotate.HandsCameraRotationsEnum.Combat, 5);




        //SetupFingers
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, true, 0.1f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, true, 0.1f);
        _playerStateMachine.AnimatingControllers.Fingers.SetUpAllFingers(_equipedWeaponSlot.WeaponData.FingersPreset.Base, 0.01f);
        _playerStateMachine.AnimatingControllers.Fingers.Discipline.SetDisciplineIk(_equipedWeaponSlot.WeaponData.FingersPreset);



        //RightHand Transform to origin
        mainPositioner.MoveRaw(_equipedWeaponSlot.WeaponData.WeaponTransforms.Origin.RightHand_Position);
        mainPositioner.RotateRaw(_equipedWeaponSlot.WeaponData.WeaponTransforms.Origin.RightHand_Rotation);

        //Change layers
        ToggleCombatLayersPreset(true, false, false, false, true, 0.4f);
        _playerStateMachine.AnimatingControllers.IkLayers.OnLerpFinish(PlayerIkLayerController.LayerEnum.RangeCombat, () =>
        {
            //Put weapon in hand
            _playerStateMachine.AnimatingControllers.WeaponHolder.RightHand(_equipedWeaponSlot.Weapon.transform);
            _playerStateMachine.AnimatingControllers.WeaponHolder.SetWeaponInHandTransform(_equipedWeaponSlot.Weapon.transform, _equipedWeaponSlot.WeaponData.InHandPosition, _equipedWeaponSlot.WeaponData.InHandRotation);


            //Move right hand to correct position
            _equipedWeaponSlot.Weapon.HoldController.MoveHandsToCurrentHoldMode(0.5f, 0.5f);
            _playerStateMachine.CombatControllers.WallDetector.ToggleCollider(true);

            _equipedWeaponSlot.Weapon.OnWeaponEquip();
        });
    }




    public void UnEquipWeapon(float unEquipSpeed)
    {
        if (!_playerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded) return;

        
        if (!IsState(CombatStateEnum.Equiped)) return;

        SetState(CombatStateEnum.UnEquip);


        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.BakedWeaponAnimating, false, 0.5f);


        _playerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(false);
        _playerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(false);

        _equipedWeaponSlot.Weapon.DamageDealingController.Toggle(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Aim.ToggleAimBool(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Block.ToggleBlockBool(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Run.ToggleRunBool(false);


        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(HudController_Crosshair.CrosshairTypeEnum.Dot);


        //Move right hand to origin
        WeaponAnimator_MainTransformer mainPositioner = _playerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainPositioner.Rotate(_equipedWeaponSlot.WeaponData.WeaponTransforms.Origin.RightHand_Rotation, 0.5f);
        mainPositioner.Move(_equipedWeaponSlot.WeaponData.WeaponTransforms.Origin.RightHand_Position, 0.5f).SetOnMoveFinish(() => 
        {
            _playerStateMachine.CombatControllers.WallDetector.ToggleCollider(false);

            //Disable fingers
            _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 0.2f);
            _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 0.2f);

            //Prepare hands camera
            _playerStateMachine.CameraControllers.Hands.Rotate.SetHandsCameraRotation(PlayerHandsCamera_Rotate.HandsCameraRotationsEnum.IdleWalkRun, 5);
            _playerStateMachine.CameraControllers.Hands.Move.SetCameraPosition(PlayerHandsCamera_Move.CameraPositionsEnum.Idle, 5);

            _equipedWeaponSlot.Weapon.OnWeaponUnEquip();
            _playerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);

            _playerStateMachine.InventoryControllers.Inventory.Weapon.HolsterWeapon(_equipedWeaponSlot.Weapon, _equipedWeaponSlot.WeaponData);
            _equipedWeaponSlot = null;


            if (_swap)
            {
                SetState(CombatStateEnum.Unarmed);
                ReEquip(_choosenWeaponIndex);
                return;
            }

            //Toggle layers
            ToggleCombatLayersPreset(false, true, true, true, false, 0.4f);
            _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.TriggerDiscipline, false, 0.2f);
            _playerStateMachine.AnimatingControllers.IkLayers.OnLerpFinish(PlayerIkLayerController.LayerEnum.RangeCombat, () =>
            {
                mainPositioner.MoveRaw(Vector3.zero);
                mainPositioner.RotateRaw(Vector3.zero);
                SetState(CombatStateEnum.Unarmed);
            });
        });
    }




    public void DropWeapon()
    {
        if (!_playerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded) return;

        if (!IsState(CombatStateEnum.Equiped)) return;
        if (_equipedWeaponSlot.WeaponData.Fists) return;


        SetState(CombatStateEnum.UnEquip);
        _playerStateMachine.CombatControllers.WallDetector.ToggleCollider(false);

        //Toggle layers
        ToggleCombatLayersPreset(false, true, true, true, false, 0.2f);



        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.BakedWeaponAnimating, false, 0.5f);


        _equipedWeaponSlot.Weapon.OnWeaponUnEquip();
        _playerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);


        //Prepare hands camera
        _playerStateMachine.CameraControllers.Hands.Rotate.SetHandsCameraRotation(PlayerHandsCamera_Rotate.HandsCameraRotationsEnum.IdleWalkRun, 5);
        _playerStateMachine.CameraControllers.Hands.Move.SetCameraPosition(PlayerHandsCamera_Move.CameraPositionsEnum.Idle, 5);


        //Disable fingers
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 0.2f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 0.2f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.TriggerDiscipline, false, 0.2f);


        //Disable bobbing/sway
        _playerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(false);
        _playerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(false);

        _equipedWeaponSlot.Weapon.DamageDealingController.Toggle(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Aim.ToggleAimBool(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Block.ToggleBlockBool(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Run.ToggleRunBool(false);

        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(HudController_Crosshair.CrosshairTypeEnum.Dot);


        _playerStateMachine.InventoryControllers.Inventory.Weapon.DropWeapon(_equipedWeaponIndex);
        _equipedWeaponSlot = null;

        SetState(CombatStateEnum.Unarmed);
    }






    private void SwapWeapon()
    {
        _swap = true;
        UnEquipWeapon(1);
    }
    private void ReEquip(int choosenWeaponIndex)
    {
        EquipWeapon(choosenWeaponIndex);
        _swap = false;
    }



    public void TemporaryUnEquip()
    {
        if (!IsState(CombatStateEnum.Equiped)) return;

        SetState(CombatStateEnum.UnEquip);
        _isTemporaryUnEquip = true;


        //Toggle layers
        ToggleCombatLayersPreset(false, false, false, false, false, 0.1f);


        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.BakedWeaponAnimating, false, 0.5f);


        _equipedWeaponSlot.Weapon.OnWeaponUnEquip();
        _playerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);


        //Disable fingers
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 0.2f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 0.2f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.TriggerDiscipline, false, 0.2f);


        //Prepare hands camera
        _playerStateMachine.CameraControllers.Hands.Rotate.SetHandsCameraRotation(PlayerHandsCamera_Rotate.HandsCameraRotationsEnum.IdleWalkRun, 5);
        _playerStateMachine.CameraControllers.Hands.Move.SetCameraPosition(PlayerHandsCamera_Move.CameraPositionsEnum.Idle, 5);


        _playerStateMachine.InventoryControllers.Inventory.Weapon.HolsterWeapon(_equipedWeaponSlot.Weapon, _equipedWeaponSlot.WeaponData);
        _equipedWeaponSlot.Weapon.DamageDealingController.Toggle(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Aim.ToggleAimBool(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Block.ToggleBlockBool(false);
        _playerStateMachine.CombatControllers.EquipedWeapon.Run.ToggleRunBool(false);

        _equipedWeaponSlot = null;

        SetState(CombatStateEnum.Unarmed);
    }
    public void RecoverFromTemporaryUnEquip()
    {
        if (_isTemporaryUnEquip) EquipWeapon(_equipedWeaponIndex);
    }


    public void ToggleCombatLayersPreset(bool combatAnim, bool spineLock, bool body, bool head, bool combat, float duration)
    {
        _playerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.CombatBase, combatAnim, duration);
        _playerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.CombatAnimating, combatAnim, duration);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.SpineLock, spineLock, duration);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Body, body, duration);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Head, head, duration);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.RangeCombat, combat, duration);
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