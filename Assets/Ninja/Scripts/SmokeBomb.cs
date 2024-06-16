using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    [SerializeField] private float smokeRadius;
    [SerializeField] private float stunDuration = 5f;
    [SerializeField] private ParticleSystem smokeParticles;

    private Collider[] colliderBuffer;
    private Rigidbody rigidBody;

    private void Awake()
    {
        colliderBuffer = new Collider[10];
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, smokeRadius, colliderBuffer);

        for (int i = 0; i < hitCount; i++)
        {
            Collider collider = colliderBuffer[i];
            if (collider.TryGetComponent(out Guard guard))
            {
                rigidBody.velocity = Vector3.zero;
                rigidBody.useGravity = true;
                guard.GetStunned(stunDuration);
                smokeParticles.Play();

                Invoke(nameof(DestroyObject), 5f);
            }
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
