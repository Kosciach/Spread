using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerLeaningController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _stateMachine;
    [SerializeField] MultiRotationConstraint[] _spineLocks;

    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] float _currentLean;
    [SerializeField] float _desiredLean;
    [Space(5)]
    [SerializeField] bool _leanInputRight;
    [SerializeField] bool _leanInputLeft;
    [Space(2)]
    [SerializeField] bool _isLean;
    [Space(5)]
    [SerializeField] int _leanDirection;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 45)]
    [SerializeField] float _leanStrenght;
    [Range(0, 5)]
    [SerializeField] float _cameraLeanStrength;
    [Space(5)]
    [Range(0, 10)]
    [SerializeField] float _leanSpeed;




    private void Update()
    {
        SetLean();
        UpdateLean();
        ApplyLean();
    }




    private void SetLean()
    {
        _isLean = _leanInputLeft || _leanInputRight;
        int leanWeight = (_isLean ? 1 : 0) * _leanDirection;

        _desiredLean = _leanStrenght * leanWeight;
    }
    private void UpdateLean()
    {
        _currentLean = Mathf.Lerp(_currentLean, _desiredLean, _leanSpeed * Time.deltaTime);
    }
    private void ApplyLean()
    {
        foreach (MultiRotationConstraint spineLock in _spineLocks)
            spineLock.data.offset = new Vector3(0, 0, _currentLean);

        _stateMachine.CameraControllers.Hands.Lean.SetRotation(new Vector3(0, 0, -(_currentLean/10) * _cameraLeanStrength));
    }


    public void LeanRight()
    {
        _leanInputRight = true;
        _leanDirection = -1;
    }
    public void LeanLeft()
    {
        _leanInputLeft = true;
        _leanDirection = 1;
    }
    public void StopLeanRight()
    {
        _leanInputRight = false;
    }
    public void StopLeanLeft()
    {
        _leanInputLeft = false;
    }
}
