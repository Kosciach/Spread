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
        if (_active) return;

        _uiDisableInputs.Enable();
        ToggleTable(true);
    }
    public void Highlight()
    {
        _outline.OutlineWidth = 4;
    }
    public void UnHighlight()
    {
        _outline.OutlineWidth = 0;
    }



    public void ToggleTable(bool enable)
    {
        _active = enable;

        _playerStateMachine.CoreControllers.Input.Toggle(!enable);
        _playerStateMachine.CameraControllers.Cine.ToggleCineInput(!enable);

        CursorLockMode cursorLockMode = enable ? CursorLockMode.None : CursorLockMode.Locked;
        _playerStateMachine.CameraControllers.Cine.SetCursorState(cursorLockMode, enable);


        CanvasController.Instance.ToggleBloom(enable);
        CanvasController.Instance.AttachmentTable.Toggle.Toggle(enable);
        CanvasController.Instance.HudControllers.Stats.Toggle.Toggle(!enable);
    }
}