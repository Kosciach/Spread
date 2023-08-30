using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] Transform _bottomRayTransform;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] float _detectedWallHeight;                 public float DetectedWallHeight { get { return _detectedWallHeight; } }
    [SerializeField] bool _isVault;                             public bool IsVault { get { return _isVault; } }
    [SerializeField] Vector3 _finalClimbPosition;               public Vector3 FinalClimbPosition { get { return _finalClimbPosition; } }
    [SerializeField] Vector3 _startClimbPosition;               public Vector3 StartClimbPosition { get { return _startClimbPosition; } }


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] LayerMask _climbMask;
    [SerializeField] float _maxClimbHeight;
    [SerializeField] float _wallDetectionRange;
    [SerializeField] float _rayCount;
    [SerializeField] float _rayDencity;






    public bool CanClimbWall()
    {
        RaycastHit detectRayHit = new RaycastHit();
        RaycastHit topRayHit = new RaycastHit();
        Vector3 topRayPosition;
        float topRayDetectionRange;
        int missedRaysCount = 0;


        if (_playerStateMachine.CoreControllers.Input.MovementInputVector.z <= 0) return false;

        for(int i=0; i<_rayCount; i++)
        {
            Debug.DrawRay(transform.position + new Vector3(0, i / _rayDencity, 0), transform.forward * _wallDetectionRange, Color.blue, 5);
            if(Physics.Raycast(transform.position + new Vector3(0, i / _rayDencity, 0), transform.forward, out detectRayHit, _wallDetectionRange, _climbMask)) break;
            missedRaysCount++;
        }


        if (missedRaysCount == _rayCount) return false;

        _isVault = detectRayHit.transform.CompareTag("Vault");
        topRayPosition = detectRayHit.point + new Vector3(0, _maxClimbHeight - (detectRayHit.point.y - transform.position.y), 0);
        topRayDetectionRange = _maxClimbHeight;

        Debug.DrawRay(topRayPosition + transform.forward / 4, Vector3.down * topRayDetectionRange, Color.green, 5);
        if (!Physics.Raycast(topRayPosition + transform.forward / 4, Vector3.down, out topRayHit, topRayDetectionRange, _climbMask)) return false;

        _startClimbPosition = new Vector3(detectRayHit.point.x, transform.position.y, detectRayHit.point.z) - transform.forward / 1.2f;
        _finalClimbPosition = topRayHit.point + transform.forward / 2;
        _detectedWallHeight = topRayHit.point.y - transform.position.y + 0.08f;

        return _detectedWallHeight > 0.5f && _detectedWallHeight <= 3.5f;
    }





    public bool CheckFallingClimb()
    {
        RaycastHit bottomRayHit;
        RaycastHit topRayHit;
        Vector3 topRayPosition;
        float topRayDetectionRange;


        Debug.DrawRay(transform.position + Vector3.up * 3, transform.forward * _wallDetectionRange / 2, Color.magenta, 5);
        if (!Physics.Raycast(transform.position + Vector3.up * 3, transform.forward, out bottomRayHit, _wallDetectionRange / 2, _climbMask)) return false;

        topRayPosition = bottomRayHit.point + new Vector3(0, _maxClimbHeight, 0);
        topRayDetectionRange = _maxClimbHeight;

        if (!Physics.Raycast(topRayPosition, Vector3.down, out topRayHit, topRayDetectionRange, _climbMask)) return false;

        _startClimbPosition = bottomRayHit.point - transform.forward / 1.2f;
        _finalClimbPosition = topRayHit.point + transform.forward / 2;
        _detectedWallHeight = topRayHit.point.y - bottomRayHit.point.y + 0.08f;

        return _detectedWallHeight <= 1.5f;
    }
}
