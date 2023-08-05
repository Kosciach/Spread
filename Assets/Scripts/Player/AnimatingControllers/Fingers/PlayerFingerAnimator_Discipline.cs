using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFingerAnimator_Discipline : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerFingerAnimator _fingerAnimator;
    [SerializeField] Transform _target;
    [SerializeField] Transform _hint;




    public void SetDisciplineIk(FingerPreset fingerPreset)
    {
        LeanTween.moveLocal(_target.gameObject, fingerPreset.TriggerDisciplineIndexFinger.Target_Position, 0.1f);
        LeanTween.rotateLocal(_target.gameObject, fingerPreset.TriggerDisciplineIndexFinger.Target_Rotation, 0.1f);

        LeanTween.moveLocal(_hint.gameObject, fingerPreset.TriggerDisciplineIndexFinger.Hint_Position, 0.1f);
    }
}
