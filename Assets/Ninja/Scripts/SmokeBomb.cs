using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    [SerializeField]
    private float smokeRadius;

    private Collider[] colliderBuffer;

    private void Awake()
    {
        colliderBuffer = new Collider[10];
    }

    private void OnCollisionEnter(Collision collision)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, smokeRadius, colliderBuffer);

        for (int i = 0; i < hitCount; i++)
        {
            Collider collider = colliderBuffer[i];
            if (collider.TryGetComponent(out Guard guard))
            {
                guard.GetSmoked();
            }
        }
    }
}
