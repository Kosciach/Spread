using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloadAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [Space(10)]
    [SerializeField] Transform _rightHandIk;
    [SerializeField] Transform _rightHandIk_Hint;
    [Space(5)]
    [SerializeField] Transform _leftHandIk;
    [SerializeField] Transform _leftHandIk_Hint;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isReloading; public bool IsReloading { get { return _isReloading; } }






    public void Reload(AnimatorOverrideController reloadAnimOveride)
    {
        if (!_playerStateMachine.MovementControllers.VerticalVelocity.GravityController.IsGrounded) return;
        if (_playerStateMachine.CombatControllers.Combat.EquipedWeaponController.Wall.IsWall) return;

        _isReloading = true;
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.WeaponReload, true, 10);

        _playerStateMachine.AnimatingControllers.Animator.OverrideAnimationClip(reloadAnimOveride);
        _playerStateMachine.AnimatingControllers.Animator.SetBool("Reload", true);

    }



    public void ReloadFinish()
    {
        _isReloading = false;
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.WeaponReload, false, 5);

        _playerStateMachine.AnimatingControllers.Animator.ResetAnimatorController();
    }









}
