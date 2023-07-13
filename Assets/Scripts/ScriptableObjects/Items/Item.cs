using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Item", fileName = "Item")]
public class Item : ScriptableObject
{
    [Header("====BaseData====")]
    public string Name;
}
