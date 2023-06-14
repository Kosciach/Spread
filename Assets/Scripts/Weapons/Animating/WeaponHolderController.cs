using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [Space(5)]
    [SerializeField] Transform _weaponHolder_R;
    [SerializeField] Transform _weaponHolder_L;







    public void RightHand(Transform weaponTransform)
    {
        weaponTransform.parent = _weaponHolder_R;
    }
    public void LeftHand(Transform weaponTransform)
    {
        weaponTransform.parent = _weaponHolder_L;
    }




    public void SetWeaponInHandTransform(Transform weaponTransform, Vector3 pos, Vector3 rot)
    {
        weaponTransform.localPosition = pos;
        weaponTransform.localRotation = Quaternion.Euler(rot);
    }
}
