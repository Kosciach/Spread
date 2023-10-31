using UnityEngine;

public class PlayerVelocityCalculator : MonoBehaviour
{
    [SerializeField] Vector3 _currentVelocity; public Vector3 CurrentVelocity { get { return _currentVelocity; } }
    private Vector3 _previousPosition;


    private void Start()
    {
        _previousPosition = transform.position;
    }
    private void Update()
    {
        _currentVelocity = (transform.position - _previousPosition) / Time.deltaTime;
        _currentVelocity.y = 0;
        _previousPosition = transform.position;
    }
}
