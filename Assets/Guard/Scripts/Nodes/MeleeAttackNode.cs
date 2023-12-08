using BehaiviourTree;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackNode : Node
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private readonly float attackDuration;

    private float timer;

    public MeleeAttackNode(NavMeshAgent agent, Animator animator, float attackDuration)
    {
        this.agent = agent;
        this.animator = animator;
        this.attackDuration = attackDuration;
    }

    public override void OnEnter()
    {
        animator.SetTrigger("Attack");
        agent.isStopped = true;
        timer = attackDuration + Time.time;
        Status = NodeStatus.Running;
    }

    public override void OnUpdate()
    {
        if (Time.time > timer)
        {
            agent.isStopped = false;
            Status = NodeStatus.Success;
        }
        Status = NodeStatus.Running;
    }
}

