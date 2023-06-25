using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFireModesAnimator : MonoBehaviour
{
    [Header("====Settings====")]
    [SerializeField] bool _toggle;
    [SerializeField] SelectorData[] _selectorDatas;


    [System.Serializable]
    private struct SelectorData
    {
        public string name;
        [Space(5)]
        public GameObject Selector;
        [Space(5)]
        public Vector3 Pos;
        public Vector3 Rot;
        [Space(5)]
        public bool Toggle;
    }





    public void OnFireModeChange(int index)
    {
        if (!_toggle) return;

        SelectorData selectorDatas = _selectorDatas[index];
        if (!selectorDatas.Toggle) return;

        LeanTween.moveLocal(selectorDatas.Selector, selectorDatas.Pos, 0.1f);
        LeanTween.rotateLocal(selectorDatas.Selector, selectorDatas.Rot, 0.1f);
    }
}
