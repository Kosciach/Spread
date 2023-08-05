using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSwimController : MonoBehaviour
{
    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] PlayerStateMachine _stateMachine;
    [SerializeField] Camera _playerMainCamera;
    [SerializeField] Volume _underWaterEffect;




    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isInWater;
    [Range(-1, 1)]
    [SerializeField] int _enableUnderWaterEffect;
    [SerializeField] Transform _currentWater;




    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] LayerMask _waterMask;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] string _waterTag;
    [Space(5)]
    [Range(0, 5)]
    [SerializeField] float _swimPointOffset;
    [Range(0, 10)]
    [SerializeField] float _underWaterEffectSpeed;





    private void Update()
    {
        UpdateUnderWaterEffect();
    }



    private void UpdateUnderWaterEffect()
    {
        _underWaterEffect.weight += _enableUnderWaterEffect * 5 * Time.deltaTime;
        _underWaterEffect.weight = Mathf.Clamp(_underWaterEffect.weight, 0, 1);
    }



    public bool CheckIsOnSurface()
    {
        return transform.position.y + _swimPointOffset == _currentWater.position.y;
    }

    public bool CheckSwimEnter()
    {
        if (!_isInWater) return false;

        return transform.position.y + _swimPointOffset <= _currentWater.position.y;
    }

    public void ClampPosition()
    {
        float maxYPosition = _currentWater.position.y - _swimPointOffset;

        Vector3 clampedPosition = transform.position;

        clampedPosition.y = Mathf.Clamp(transform.position.y, -100, maxYPosition);
        transform.position = clampedPosition;
    }

    public bool CheckObjectInFront()
    {
        return Physics.Raycast(transform.position, transform.forward * _stateMachine.CoreControllers.Input.MovementInputVector.z, 1f, _groundMask);
    }

    public void ToggleCameraEffect(bool enable)
    {
        _enableUnderWaterEffect = enable ? 1 : -1;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_waterTag))
        {
            _isInWater = true;
            _currentWater = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_waterTag))
        {
            _isInWater = false;
        }
    }
}
