using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponAttachmentController : MonoBehaviour
{
    [Header("====Settings====")]
    [SerializeField] WeaponAttachmentSlot[] _weaponAttachmentsSlots;
}


[System.Serializable]
public class WeaponAttachmentSlot
{
    public string Name;
    public AttachmentTypes AttachmentType;
    public GameObject SlotGameObject;


    public enum AttachmentTypes
    {
        None, Sight, Muzzle, Magazine
    }
}