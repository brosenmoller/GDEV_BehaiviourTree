using UnityEngine;
using UnityEngine.AI;

namespace BehaiviourTree
{
    public class MoveToTargetTransformNode : Node
    {
        private readonly NavMeshAgent agent;
        private readonly float moveSpeed;
        private readonly float stoppingDistance;

        private Transform targetTransform;

        public MoveToTargetTransformNode(NavMeshAgent agent, float moveSpeed, float stoppingDistance)
        {
            this.agent = agent;
            this.moveSpeed = moveSpeed;
            this.stoppingDistance = stoppingDistance;
        }

        public override void OnEnter()
        {
            targetTransform = blackboard.GetVariable<Transform>(VariableNames.TARGET_Transform);

            if (targetTransform == null || agent == null) { Status = NodeStatus.Failed; return; }

            agent.speed = moveSpeed;
            agent.isStopped = false;
            agent.stoppingDistance = stoppingDistance;
            Status = NodeStatus.Running;
        }

        public override void OnUpdate()
        {
            if (agent.pathPending) { Status = NodeStatus.Running; return; }
            if (agent.hasPath && agent.path.status == NavMeshPathStatus.PathInvalid) { Status = NodeStatus.Failed; return; }
            if (agent.pathEndPosition != targetTransform.position)
            {
                agent.SetDestination(targetTransform.position);
            }

            if (Vector3.Distance(agent.transform.position, targetTransform.position) <= stoppingDistance)
            {
                Status = NodeStatus.Success;
                return;
            }

            Status = NodeStatus.Running;
        }

        public override void OnExit()
        {
            Debug.Log("Patrol Exit");
            agent.isStopped = true;
        }
    }
}