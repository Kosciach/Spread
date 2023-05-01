using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _stateMachine;
    [SerializeField] CharacterController _characterController;




    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Vector3 _dashDirection;
    [Range(0, 1)]
    [SerializeField] float _dashSlowDown;




    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 2)]
    [SerializeField] float _dashDuration;
    [Range(0, 10)]
    [SerializeField] int _dashSpeed;




    public void DashStart(Vector3 dashDirection)
    {
        _dashDirection = dashDirection;
        _dashSlowDown = 1;

        LeanTween.value(_dashSlowDown, 0, _dashDuration).setOnUpdate((float val) =>
        {
            _dashSlowDown = val;
        }).setOnComplete(() =>
        {
            _stateMachine.SwitchController.SwitchTo.Idle();
        });
    }
    public void DashMove()
    {
        _characterController.Move(_dashDirection * _dashSpeed * 10 * Time.deltaTime * _dashSlowDown);
    }
}
