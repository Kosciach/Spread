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


    [Space(10)]
    [Header("====Stackable====")]
    public bool Stackable;
    [Range(2, 100)]
    public int MaxCountPerSlot = 2;
}
