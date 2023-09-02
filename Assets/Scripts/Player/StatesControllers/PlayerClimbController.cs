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

    [Space(5)]
    [SerializeField] float _detectRayCount;
    [SerializeField] float _detectRayDencity;
 
    [Space(5)]
    [SerializeField] float _topRayCount;
    [SerializeField] float _topRayDencity;



    public bool CanClimbWall()
    {
        RaycastHit detectRayHit = new RaycastHit();
        RaycastHit topRayHit = new RaycastHit();
        int missedDetectRayCount = 0;
        int hitTopRayCount = 0;


        //Check for obsticle. If at least one ray hits, there is something, if all rays missed nothing was detected.
        if (_playerStateMachine.CoreControllers.Input.MovementInputVector.z <= 0) return false;

        for(int i=0; i<_detectRayCount; i++)
        {
            Debug.DrawRay(transform.position + new Vector3(0, i / _detectRayDencity, 0), transform.forward * _wallDetectionRange, Color.blue, 5);
            if(Physics.Raycast(transform.position + new Vector3(0, i / _detectRayDencity, 0), transform.forward, out detectRayHit, _wallDetectionRange, _climbMask)) break;
            missedDetectRayCount++;
        }

        if (missedDetectRayCount == _detectRayCount) return false;


        //Check if obsticle is of correct height and if it should be climbed or vaulted over.
        Vector3 rayPos = new Vector3(detectRayHit.point.x, transform.position.y, detectRayHit.point.z) + (Vector3.up * _maxClimbHeight);
        Debug.DrawRay(rayPos, Vector3.down * _maxClimbHeight, Color.green, 5);
        Physics.Raycast(rayPos, Vector3.down, out topRayHit, _maxClimbHeight, _climbMask);

        for (int i=1; i<_topRayCount; i++)
        {
            rayPos = new Vector3(detectRayHit.point.x, transform.position.y, detectRayHit.point.z) + (Vector3.up * _maxClimbHeight) + (transform.forward * i / _topRayDencity);
            Debug.DrawRay(rayPos, Vector3.down * _maxClimbHeight, Color.green, 5);
            if(Physics.Raycast(rayPos, Vector3.down, _maxClimbHeight, _climbMask)) hitTopRayCount++;
        }
        if (hitTopRayCount == 0) return false;

        _startClimbPosition = new Vector3(detectRayHit.point.x, transform.position.y, detectRayHit.point.z) - transform.forward / 1.2f;
        _finalClimbPosition = topRayHit.point + transform.forward / 2;
        _detectedWallHeight = topRayHit.point.y - transform.position.y + 0.08f;

        _isVault = hitTopRayCount > 0 && hitTopRayCount <= 7 && _detectedWallHeight > 1.1f && _detectedWallHeight <= 2;

        return _detectedWallHeight >= 0.2f && _detectedWallHeight <= 5;
    }




    public bool CheckFallClimb()
    {
        RaycastHit bottomRayHit;
        RaycastHit topRayHit;
        Vector3 topRayPosition;
        float topRayDetectionRange;


        Debug.DrawRay(transform.position + Vector3.up * 2.5f, transform.forward * _wallDetectionRange / 2, Color.magenta, 5);
        if (!Physics.Raycast(transform.position + Vector3.up * 2.5f, transform.forward, out bottomRayHit, _wallDetectionRange / 2, _climbMask)) return false;

        topRayPosition = bottomRayHit.point + Vector3.up/2;
        topRayDetectionRange = 0.5f;

        Debug.DrawRay(topRayPosition, Vector3.down * topRayDetectionRange, Color.green, 5);
        if (!Physics.Raycast(topRayPosition, Vector3.down, out topRayHit, topRayDetectionRange, _climbMask)) return false;

        _startClimbPosition = bottomRayHit.point - transform.forward / 1.2f;
        _finalClimbPosition = topRayHit.point + transform.forward / 2;
        _detectedWallHeight = topRayHit.point.y - bottomRayHit.point.y + 0.08f;

        return _detectedWallHeight <= 1.5f;
    }
}
