using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ammo", fileName = "Ammo")]
public class Ammo : ScriptableObject
{
    public int SizeInInventory;
    public AmmoTypeEnum AmmoType;




    public enum AmmoTypeEnum
    {
        Handgun, Smg, Rifle, HeavyRifle
    }
}
