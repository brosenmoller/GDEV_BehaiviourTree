using BehaiviourTree;
using UnityEngine;
using System.Collections.Generic;

public class DetectObjectsNode : Node
{
    private readonly Transform controllerTransform;
    private readonly float viewRadius;
    private readonly float viewAngle;
    private readonly LayerMask targetMask;
    private readonly LayerMask obstacleMask;

    private readonly Collider[] targetsInViewRadius;
    private readonly List<Transform> visibleTargets;

    public DetectObjectsNode(Transform controllerTransform, float viewRadius, float viewAngle, LayerMask targetMask, LayerMask obstacleMask)
    {
        this.controllerTransform = controllerTransform;
        this.viewRadius = viewRadius;
        this.viewAngle = viewAngle;
        this.targetMask = targetMask;
        this.obstacleMask = obstacleMask;

        targetsInViewRadius = new Collider[20];
        visibleTargets = new List<Transform>();
    }

    // Adapted from https://github.com/SebLague/Field-of-View/blob/master/Episode%2003/FieldOfView.cs
    private void FindVisibleTargets()
    {
        visibleTargets.Clear();
        int numberOfDetectedColliders = Physics.OverlapSphereNonAlloc(controllerTransform.position, viewRadius, targetsInViewRadius, targetMask);

        for (int i = 0; i < numberOfDetectedColliders; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - controllerTransform.position).normalized;
            if (Vector3.Angle(controllerTransform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(controllerTransform.position, target.position);
                if (!Physics.Raycast(controllerTransform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public override void OnEnter()
    {
        FindVisibleTargets();

        blackboard.SetVariable(VariableNames.VISIBLE_TARGETS_TransformArray, visibleTargets.ToArray());

        if (visibleTargets.Count > 0)
        {
            Status = NodeStatus.Success;
            return;
        }

        Status = NodeStatus.Failed;
    }
}

