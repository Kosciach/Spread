using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsControllers_Switch : MonoBehaviour
{
    private GameObject[] _panels;


    public enum PanelTypes
    {
        Map, Inventory, Craft, SkillTree
    }


    private void Awake()
    {
        _panels = new GameObject[4];
        for (int i = 0; i < 4; i++) _panels[i] = transform.parent.GetChild(i+1).gameObject;
    }

    public void SwitchPanel(int index)
    {
        foreach (GameObject panel in _panels) panel.SetActive(false);
        _panels[index].SetActive(true);
    }
    public void SwitchPanel(PanelTypes index)
    {
        SwitchPanel((int)index);
    }
}
