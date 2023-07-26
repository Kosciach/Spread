using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseThrowableController : MonoBehaviour
{
    protected ThrowableStateMachine _stateMachine;


    private void Awake()
    {
        _stateMachine = GetComponent<ThrowableStateMachine>();
        OnAwake();
    }



    protected virtual void OnAwake() { }
    public abstract void OnSafe();
    public abstract void OnActivate();




    protected void Explode(GameObject explosionParticle, float explosionRadius, float shakeStrength, float explosionForce)
    {
        Instantiate(explosionParticle, transform.position, Quaternion.Euler(-90, 0, 0));
        CameraShake.Instance.Shake(shakeStrength, 5);

        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider detectedObject in detectedObjects)
        {
            Rigidbody detectedRigidbody = detectedObject.GetComponent<Rigidbody>();
            if (detectedRigidbody != null) detectedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            IDamageable damageable = detectedObject.GetComponent<IDamageable>();
            if (damageable != null) damageable.TakeDamage(_stateMachine.ThrowableData.Damage);
        }
    }
}
