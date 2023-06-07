using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShellController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Rigidbody _rigidbody;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _ejectRightForce;
    [Range(0, 10)]
    [SerializeField] float _ejectBackForce;
    [Space(5)]
    [Range(0, 90)]
    [SerializeField] float _rotationStrength;


    private float _rotation;



    private void Awake()
    {
        _rotation = Random.Range(-_rotationStrength, _rotationStrength);
        transform.LeanScale(Vector3.zero, 1);
    }
    private void Start()
    {
        Destroy(gameObject, 3);
    }
    private void Update()
    {
        Rotate();
    }



    public void PassData(float playerSideMovement)
    {
        playerSideMovement = Mathf.Clamp(playerSideMovement, 0, 1);
        Vector3 additionalEjectVector = -transform.up * playerSideMovement * 5;

        Vector3 ejectVector = (-transform.up * _ejectRightForce + additionalEjectVector) + (transform.right * _ejectBackForce);
        _rigidbody.AddForce(ejectVector, ForceMode.Impulse);
    }
    private void Rotate()
    {
        transform.Rotate(transform.forward, _rotation * 10 * Time.deltaTime);
        transform.Rotate(transform.up, _rotation * 5 * Time.deltaTime);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") || other.CompareTag("Player")) return;
        Destroy(gameObject);
    }



    private void OnDestroy()
    {
        //Play sound
    }
}
