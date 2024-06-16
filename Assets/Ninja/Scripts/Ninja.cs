using BehaiviourTree;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Ninja : MonoBehaviour
{
    [Header("Ninja Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stoppingDistance;

    [Header("Follow Settings")]
    [SerializeField] private float followDistance;

    [Header("Hide")]
    [SerializeField] private Transform[] ninjaCoverSpots;
    [SerializeField] private GameObject smokeBomb;

    [Header("References")]
    [SerializeField] private StateVisualizer stateVisualizer;

    private NavMeshAgent agent;
    private Animator animator;

    private Node tree;
    public BlackBoard blackBoard;
    private Transform playerTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = FindObjectOfType<PlayerMovement>().transform;

        blackBoard = new BlackBoard();
        blackBoard.SetVariable(VariableNames.PLAYER_CHASED_Bool, false);

        Node followPlayerTree = new ConditionNode(
            new SequenceNode(
                new ActionExecuterNode(() => stateVisualizer.SetText("Following Player")),
                new ActionExecuterNode(() => blackBoard.SetVariable(VariableNames.TARGET_POSITION_Vec3, playerTransform.position)),
                new MoveToTargetPositionNode(agent, moveSpeed, followDistance)
            ),
            () => Vector3.Distance(playerTransform.position, transform.position) > followDistance + followDistance / 4
        );

        Node hideTree = new ConditionNode(
            new SequenceNode(
                new ActionExecuterNode(() => stateVisualizer.SetText("Hiding")),
                new ActionExecuterNode(() => blackBoard.SetVariable(VariableNames.TARGET_POSITION_Vec3, GetClosestCover().position)),
                new MoveToTargetPositionNode(agent, moveSpeed, followDistance),
                new ThrowObjectNode(smokeBomb)
            ),
            () => blackBoard.GetVariable<bool>(VariableNames.PLAYER_CHASED_Bool)
        );

        Node idleTree = new SequenceNode(
            new ActionExecuterNode(() => stateVisualizer.SetText("Idling")),
            new ActionExecuterNode(() => agent.isStopped = true)
         );

        tree = new SelectorNode(hideTree, followPlayerTree, idleTree);

        tree.SetupBlackboard(blackBoard);
    }

    private Transform GetClosestCover()
    {
        float shortest = Mathf.Infinity;
        int shortestCoverIndex = 0;

        for (int i = 0; i < ninjaCoverSpots.Length; i++)
        {
            float distance = (ninjaCoverSpots[i].position - transform.position).sqrMagnitude;
            if (distance < shortest)
            {
                shortest = distance;
                shortestCoverIndex = i;
            }
        }

        return ninjaCoverSpots[shortestCoverIndex];
    }

    private void FixedUpdate()
    {
        if (agent.isStopped)
        {
            animator.SetBool("Moving", false);
        }
        else
        {
            animator.SetBool("Moving", true);
        }

        tree.Tick();
    }
}
