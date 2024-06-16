using BehaiviourTree;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackNode : Node
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private readonly Timer timer;

    public MeleeAttackNode(NavMeshAgent agent, Animator animator, float attackDuration)
    {
        this.agent = agent;
        this.animator = animator;

        timer = new Timer(attackDuration);
    }

    public override void OnEnter()
    {
        animator.SetTrigger("Attack");
        agent.isStopped = true;

        timer.Restart();
        Status = NodeStatus.Running;
    }

    public override void OnUpdate()
    {
        if (timer.IsFinished)
        {
            agent.isStopped = false;
            Status = NodeStatus.Success;
        }
        else
        {
            Status = NodeStatus.Running;
        }
    }
}
