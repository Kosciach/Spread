using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItem_Count : MonoBehaviour
{
    private TextMeshProUGUI _count;


    private void Awake()
    {
        _count = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }








    public void UpdateCount(int count)
    {
        _count.text = count.ToString();
        _count.color = new Color(1, 1, 1, 1);

        if (count > 1) return;
        _count.color = new Color(1, 1, 1, 0);
    }
}
