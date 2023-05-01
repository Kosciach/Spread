using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ScriptableObject
{
    public string WeaponName;
    public WeaponHolderEnum WeaponHolder;







    public enum WeaponHolderEnum
    { 
        Primary, Secondary, Melee_Big, Melee_Medium, Melee_Small
    }
}
