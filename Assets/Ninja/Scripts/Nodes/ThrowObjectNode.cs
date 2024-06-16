using BehaiviourTree;
using UnityEngine;

public class ThrowObjectNode : Node
{
    private readonly GameObject objectPrefab;
    private readonly Transform origin;
    private readonly float throwSpeed;

    private Transform target;
    private Timer timer;

    public ThrowObjectNode(GameObject objectPrefab, Transform origin, float throwSpeed, float throwInterval)
    {
        this.objectPrefab = objectPrefab;
        this.origin = origin;
        this.throwSpeed = throwSpeed;

        timer = new Timer(throwInterval);
    }

    public override void OnEnter()
    {
        target = blackboard.GetVariable<Transform>(VariableNames.THROW_TARGET_Transfom);
        timer.Restart();
    }

    public override void OnUpdate()
    {
        if (timer.IsFinished)
        {
            ThrowObject();
            timer.Restart();
        }
    }

    private void ThrowObject()
    {
        if (objectPrefab != null && target != null)
        {
            GameObject thrownObject = Object.Instantiate(objectPrefab, origin.position, Quaternion.identity);
            
            if (thrownObject.TryGetComponent(out Rigidbody rigidBody))
            {
                Vector3 direction = (target.position - origin.position).normalized;
                rigidBody.AddForce(direction * throwSpeed, ForceMode.VelocityChange);
                
                Status = NodeStatus.Success;
            }
            else
            {
                Debug.LogError("ThrowObjectNode: The instantiated object does not have a Rigidbody component.");
                Status = NodeStatus.Failed;
            }
        }
        else
        {
            Debug.LogError("ThrowObjectNode: Object prefab or target is null.");
            Status = NodeStatus.Failed;
        }
    }
}
