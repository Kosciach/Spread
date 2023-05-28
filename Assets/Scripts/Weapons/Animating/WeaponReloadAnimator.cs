using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloadAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isReloading; public bool IsReloading { get { return _isReloading; } }

    private Transform _rightHandIk => _playerStateMachine.AnimatingControllers.Weapon.RightHandIk;
    private Transform _leftHandIk => _playerStateMachine.AnimatingControllers.LeftHand.LeftHandIk;





    public void Reload(ReloadAnim reloadAnim)
    {
        if (!_playerStateMachine.MovementControllers.VerticalVelocity.GravityController.IsGrounded) return;
        if (_playerStateMachine.CombatControllers.Combat.EquipedWeaponController.Wall.IsWall) return;

        _playerStateMachine.CombatControllers.Combat.EquipedWeaponController.Aim.ToggleAimBool(false);

        _isReloading = true;

        reloadAnim.Play(_rightHandIk, _leftHandIk);

        StartCoroutine(Siema());
    }




    private IEnumerator Siema()
    {
        yield return new WaitForSeconds(2);

        _isReloading = false;
    }
}
