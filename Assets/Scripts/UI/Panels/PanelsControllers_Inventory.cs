using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsControllers_Inventory : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _uiSlotsMainHolder;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] GameObject _uiSlotPrefab;



    public GameObject CreateUISlot()
    {
        return Instantiate(_uiSlotPrefab, _uiSlotsMainHolder);
    }
}
