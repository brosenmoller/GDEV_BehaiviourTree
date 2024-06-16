using BehaiviourTree;
using UnityEngine;

public class ThrowObjectNode : Node
{
    private readonly GameObject objectPrefab;
    private Transform target;

    public ThrowObjectNode(GameObject objectPrefab)
    {
        this.objectPrefab = objectPrefab;
    }

    public override void OnEnter()
    {
        target = blackboard.GetVariable<Transform>(VariableNames.THROW_TARGET_Transfom);
        Debug.Log("Throw Bomb");
    }
}
