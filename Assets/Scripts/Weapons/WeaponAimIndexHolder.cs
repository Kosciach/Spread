using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAimIndexHolder : MonoBehaviour
{
    [SerializeField] int _weaponAimIndex; public int WeaponAimIndex { get { return _weaponAimIndex; } set { _weaponAimIndex = value; } }
}
