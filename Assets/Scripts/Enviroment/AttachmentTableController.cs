using IkLayers;
using SimpleMan.CoroutineExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentTableController : MonoBehaviour, IInteractable, IHighlightable
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] Outline _outline;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _active;


    private UIDisableInputs _uiDisableInputs;



    private void Awake()
    {
        _uiDisableInputs = new UIDisableInputs();
    }
    private void Start()
    {
        _uiDisableInputs.AttachmentTable.Escape.performed += ctx =>
        {
            ToggleTable(false);
            _uiDisableInputs.Disable();
        };
    }


    public void Interact()
    {
        if (_active
            || !_playerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded
            || !_playerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equiped)) return;

        _uiDisableInputs.Enable();
        ToggleTable(true);
    }
    public void Highlight()
    {
        if (!_playerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equiped)) return;
        _outline.OutlineWidth = 4;
    }
    public void UnHighlight()
    {
        _outline.OutlineWidth = 0;
    }



    public void ToggleTable(bool enable)
    {
        _active = enable;


        _playerStateMachine.CameraControllers.Cine.ToggleCineInput(!enable);


        CursorLockMode cursorLockMode = enable ? CursorLockMode.None : CursorLockMode.Locked;
        _playerStateMachine.CameraControllers.Cine.SetCursorState(cursorLockMode, enable);


        CanvasController.Instance.HudControllers.Stats.Toggle.Toggle(!enable);
        CanvasController.Instance.HudControllers.Crosshair.Toggle.Toggle(!enable);


        Action toggleMethod = enable ? EnableTable : DisableTable;
        toggleMethod();
    }

    private void EnableTable()
    {
        _playerStateMachine.SwitchController.SwitchTo.AttachmentTable();

        Transform weaponTransform = _playerStateMachine.CombatControllers.Combat.EquipedWeaponSlot.Weapon.transform;
        _playerStateMachine.CombatControllers.Combat.TemporaryUnEquip();
        weaponTransform.parent = transform.GetChild(2);
        weaponTransform.localPosition = Vector3.zero;
        weaponTransform.localRotation = Quaternion.Euler(new Vector3(90, 180, 0));


        _playerStateMachine.transform.LeanMove(transform.GetChild(0).position, 0.2f);


        float angle = transform.rotation.eulerAngles.y - 180;
        int angleSignCorrector = (int)Mathf.Sign(_playerStateMachine.CameraControllers.Cine.CinePOV.m_HorizontalAxis.Value);
        angle = Mathf.Abs(angle) * angleSignCorrector;

        _playerStateMachine.CameraControllers.Cine.Horizontal.RotateToAngle(angle, 1);
        _playerStateMachine.CameraControllers.Cine.Horizontal.ToggleWrap(false);
        _playerStateMachine.CameraControllers.Cine.Vertical.RotateToAngle(70, 1);
        _playerStateMachine.CameraControllers.Cine.Move.SetCameraPosition(PlayerCineCamera_Move.CameraPositionsEnum.AttachmentTable, 0.5f);


        _playerStateMachine.CoreControllers.Collider.SetColliderRadius(0.1f, 0.2f);
    }
    private void DisableTable()
    {
        _playerStateMachine.CameraControllers.Cine.Horizontal.ToggleWrap(true);

        _playerStateMachine.transform.LeanMove(transform.GetChild(1).position, 0.5f);
        _playerStateMachine.CameraControllers.Cine.Move.SetCameraPosition(PlayerCineCamera_Move.CameraPositionsEnum.OnGround, 0.2f);

        _playerStateMachine.CombatControllers.Combat.RecoverFromTemporaryUnEquip();

        _playerStateMachine.CameraControllers.Cine.Vertical.RotateToAngle(0, 0.5f);
        _playerStateMachine.CameraControllers.Cine.Vertical.SetRotateOnFinish(() =>
        {
            _playerStateMachine.SwitchController.SwitchTo.Idle();
        });
    }
}