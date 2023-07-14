using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Item", fileName = "ItemData")]
public class ItemData : ScriptableObject
{
    [Header("====BaseData====")]
    public string ItemName;
    public Sprite Icon;
}
