using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryState : PlayerBaseState
{

    public PlayerInventoryState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CombatControllers.Combat.TemporaryUnEquip.StartTemporaryUnEquip(true, 0.5f);
        _ctx.CameraControllers.Cine.ToggleCineInput(false);
        _ctx.CameraControllers.Cine.SetCursorState(CursorLockMode.None, true);

        CanvasController.Instance.PanelsControllers.Switch.SwitchPanel(PanelsControllers_Switch.PanelTypes.Inventory);
        CanvasController.Instance.ToggleBloom(true);
        CanvasController.Instance.HudControllers.MainToggle.Toggle(false);
        CanvasController.Instance.PanelsControllers.MainToggle.Toggle(true);
    }
    public override void StateUpdate()
    {

    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)) StateChange(_factory.Idle());
    }
    public override void StateExit()
    {
        CanvasController.Instance.PanelsControllers.MainToggle.Toggle(false);
        CanvasController.Instance.HudControllers.MainToggle.Toggle(true);
        CanvasController.Instance.ToggleBloom(false);

        _ctx.CameraControllers.Cine.SetCursorState(CursorLockMode.Locked, false);
        _ctx.CameraControllers.Cine.ToggleCineInput(true);
        _ctx.CombatControllers.Combat.TemporaryUnEquip.RecoverFromTemporaryUnEquip();
    }
}
