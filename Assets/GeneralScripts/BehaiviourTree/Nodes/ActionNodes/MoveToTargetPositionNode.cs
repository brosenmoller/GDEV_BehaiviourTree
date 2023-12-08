using UnityEngine;
using UnityEngine.AI;

namespace BehaiviourTree
{
    public class MoveToTargetPositionNode : Node
    {
        private readonly NavMeshAgent agent;
        private readonly float moveSpeed;
        private readonly float stoppingDistance;

        private Vector3 targetPosition;

        public MoveToTargetPositionNode(NavMeshAgent agent, float moveSpeed, float stoppingDistance)
        {
            this.agent = agent;
            this.moveSpeed = moveSpeed;
            this.stoppingDistance = stoppingDistance;
        }

        public override void OnEnter()
        {
            targetPosition = blackboard.GetVariable<Vector3>(VariableNames.TARGET_POSITION_Vec3);

            if (targetPosition == null || agent == null) { Status = NodeStatus.Failed; return; }

            agent.speed = moveSpeed;
            agent.stoppingDistance = stoppingDistance;
            agent.isStopped = false;
            Status = NodeStatus.Running;
        }

        public override void OnUpdate()
        {
            if (agent.pathPending) { Status = NodeStatus.Running; return; }
            if (agent.hasPath && agent.path.status == NavMeshPathStatus.PathInvalid) { Status = NodeStatus.Failed; return; }
            if (agent.pathEndPosition != targetPosition)
            {
                agent.SetDestination(targetPosition);
            }

            if (Vector3.Distance(agent.transform.position, targetPosition) <= stoppingDistance)
            {
                Status = NodeStatus.Success;
                return;
            }

            Status = NodeStatus.Running;
        }

        public override void OnExit()
        {
            agent.isStopped = true;
        }
    }
}