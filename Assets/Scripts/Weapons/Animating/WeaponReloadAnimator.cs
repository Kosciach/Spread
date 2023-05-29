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






    public void Reload(WeaponReloadAnimData reloadAnim)
    {
        if (!_playerStateMachine.MovementControllers.VerticalVelocity.GravityController.IsGrounded) return;
        if (_playerStateMachine.CombatControllers.Combat.EquipedWeaponController.Wall.IsWall) return;

        _isReloading = true;
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.WeaponReload, true, 10);

        StartCoroutine(AnimRightHand(reloadAnim));
    }



    public void ReloadFinish()
    {
        _isReloading = false;
        _playerStateMachine.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.WeaponReload, false, 10);
    }






    private IEnumerator AnimRightHand(WeaponReloadAnimData reloadAnim)
    {
        for (int i = 0; i < reloadAnim.RightHandTransforms.Length; i++)
        {
            yield return new WaitForSeconds(reloadAnim.RightHandTransforms[i].TimeToHappen);


            Debug.Log(i);
            LeanTween.rotateLocal(_rightHandIk.gameObject, reloadAnim.RightHandTransforms[i].Rot, reloadAnim.RightHandTransforms[i].Rot_Time);

            _rightHandIk.LeanMoveLocal(reloadAnim.RightHandTransforms[i].Pos, reloadAnim.RightHandTransforms[i].Pos_Time);
            _rightHandIk_Hint.LeanMoveLocal(reloadAnim.RightHandTransforms[i].Hint_Pos, reloadAnim.RightHandTransforms[i].Pos_Time);
        }
        ReloadFinish();
    }
}
