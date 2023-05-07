using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVelocity : MonoBehaviour
{
    [Header("====Debugs====")]
    [SerializeField] Vector3 _velocity; public Vector3 Velocity { get { return _velocity; } }
    [SerializeField] Vector3 _velocityAbsolute; public Vector3 VelocityAbsolute { get { return _velocityAbsolute; } }


    private Vector3 _previous;
    private Vector3 _current;



    private void Update()
    {
        _velocity = (transform.position - _previous) / Time.deltaTime;
        _previous = transform.position;

        _velocityAbsolute.x = Mathf.Abs(_velocity.x);
        _velocityAbsolute.y = Mathf.Abs(_velocity.y);
        _velocityAbsolute.z = Mathf.Abs(_velocity.z);
    }
}
