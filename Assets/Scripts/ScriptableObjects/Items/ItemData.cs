using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Item", fileName = "ItemData")]
public class ItemData : ScriptableObject
{
    [Header("====BaseData====")]
    public string ItemName;
    public Sprite Icon;
    public GameObject ItemPrefab;
    public ItemTypes ItemType;

    [Space(10)]
    [Header("====Stackable====")]
    public bool Stackable;
    [Range(2, 100)]
    public int MaxCountPerSlot = 2;


    public enum ItemTypes
    {
        Item, Throwable
    }
}
