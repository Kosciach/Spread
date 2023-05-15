using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHoldController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] protected PlayerCombatController _playerCombatController;
    protected WeaponStateMachine _stateMachine;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] protected HoldModeEnum _holdMode; public HoldModeEnum HoldMode { get { return _holdMode; } }


    public enum HoldModeEnum { Rest, Hip }
    protected delegate void HoldModeChangeMethod(float rotateSpeed, float moveSpeed);
    protected HoldModeChangeMethod[] _holdModeChangeMethods = new HoldModeChangeMethod[2];



    private void Awake()
    {
        _playerCombatController = FindObjectOfType<PlayerCombatController>();
        _stateMachine = GetComponent<WeaponStateMachine>();

        _holdModeChangeMethods[0] = RestHoldMode;
        _holdModeChangeMethods[1] = HipHoldMode;
        VirtualAwake();
    }
    protected virtual void VirtualAwake() {}







    public virtual void MoveHandsToCurrentHoldMode(float rotateSpeed, float moveSpeed)
    {
        _holdModeChangeMethods[(int)_holdMode](rotateSpeed, moveSpeed);
    }
    public virtual void ChangeHoldMode(HoldModeEnum mode) {}
    public bool IsHoldMode(HoldModeEnum mode)
    {
        return _holdMode == mode;
    }





    public virtual void RestHoldMode(float rotateSpeed, float moveSpeed) {}
    public virtual void HipHoldMode(float rotateSpeed, float moveSpeed) {}
}
