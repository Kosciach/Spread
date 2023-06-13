using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandSwitchController : MonoBehaviour
{
    [Header("====References====")]
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




    public void SetWeaponInHandPos(Transform weaponTransform, Vector3 pos)
    {
        weaponTransform.localPosition = pos;
    }
}
