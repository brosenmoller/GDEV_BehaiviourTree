using UnityEngine;
using BehaiviourTree;
using UnityEngine.AI;
using System.Linq;
using System;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Guard : MonoBehaviour
{
    [Header("Guard Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stoppingDistance;

    [Header("Waypoints")]
    [SerializeField] private Transform[] wayPoints;

    [Header("Attack")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackAngle;
    [SerializeField] private LayerMask attackableMask;
    [SerializeField] private Transform weaponHolder;

    [Header("Field Of View")]
    [SerializeField] private float viewRadius;
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField, Range(2, 100)] private int visualisationLines;

    [Header("References")]
    [SerializeField] private StateVisualizer stateVisualizer;

    private NavMeshAgent agent;
    private Animator animator;

    private Node tree;
    private BlackBoard blackBoard;
    private Transform playerTransform;

    private BlackBoard ninjaBlackboard;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = FindObjectOfType<PlayerMovement>().transform;

        blackBoard = new BlackBoard();
        blackBoard.SetVariable(VariableNames.CURRENT_WAYPOINT_INDEX_Int, 0);
        blackBoard.SetVariable<GameObject>(VariableNames.HELT_WEAPON_GameObject, null);
        blackBoard.SetVariable(VariableNames.SEARCHING_FOR_WEAPON_Bool, false);

        Node patrolTree = new SequenceNode(
            new ActionExecuterNode(() => stateVisualizer.SetText("Patrolling")),
            new ActionExecuterNode(() => ninjaBlackboard.SetVariable(VariableNames.PLAYER_CHASED_Bool, false)),
            new SetTargetToNextWaypoint(wayPoints),
            new MoveToTargetPositionNode(agent, moveSpeed, stoppingDistance)
        );

        Node playerChaseTree = new SequenceNode(
            new ActionExecuterNode(() => stateVisualizer.SetText("Chasing Player")),
            new ActionExecuterNode(() => ninjaBlackboard.SetVariable(VariableNames.PLAYER_CHASED_Bool, true)),
            new ActionExecuterNode(() => blackBoard.SetVariable(VariableNames.TARGET_TRANSFORM, playerTransform)),
            new MoveToTargetTransformNode(agent, moveSpeed, stoppingDistance)
        );

        Node playerDetectionTree = new ResettingSequenceNode(
            new DetectObjectsNode(transform, viewRadius, viewAngle, targetMask, obstacleMask),
            new ConditionNode(
                new ConditionSplitNode(
                    playerChaseTree,
                    new ActionExecuterNode(() => blackBoard.SetVariable(VariableNames.SEARCHING_FOR_WEAPON_Bool, true)),
                    () => blackBoard.GetVariable<GameObject>(VariableNames.HELT_WEAPON_GameObject) != null
                 ),
                () => blackBoard.GetVariable<Transform[]>(VariableNames.VISIBLE_TARGETS_TransformArray).Contains(playerTransform)
            )
        );

        Node pickupWeaponTree = new SequenceNode(
            new ActionExecuterNode(() => stateVisualizer.SetText("Picking up Weapon")),
            new ActionExecuterNode(() => blackBoard.SetVariable(
                    VariableNames.TARGET_POSITION_Vec3,
                    Array.Find(
                        blackBoard.GetVariable<Transform[]>(VariableNames.VISIBLE_TARGETS_TransformArray), 
                        element => element.gameObject.CompareTag("Weapon")
                    ).position
                )
            ),
            new MoveToTargetPositionNode(agent, moveSpeed, stoppingDistance),
            new PickupWeaponNode(weaponHolder)
        );

        Node weaponSearchTree = new ConditionNode(
             new ResettingSequenceNode(
                 new ActionExecuterNode(() => stateVisualizer.SetText("Searching For Weapon")),
                 new DetectObjectsNode(transform, viewRadius, 360, targetMask, obstacleMask),
                 new ConditionNode(
                    pickupWeaponTree,
                    () => Array.Exists(
                        blackBoard.GetVariable<Transform[]>(VariableNames.VISIBLE_TARGETS_TransformArray), 
                        element => element.gameObject.CompareTag("Weapon")
                    )
                )
             ),
             () => blackBoard.GetVariable<bool>(VariableNames.SEARCHING_FOR_WEAPON_Bool)
        );

        Node attackTree = new ResettingSequenceNode(
            new DetectObjectsNode(transform, attackRange, attackAngle, attackableMask, obstacleMask),
            new ConditionNode(
                new SequenceNode(
                    new ActionExecuterNode(() => stateVisualizer.SetText("Attacking")),
                    new MeleeAttackNode(agent, animator, 1f)
                ),
                () => blackBoard.GetVariable<Transform[]>(VariableNames.VISIBLE_TARGETS_TransformArray).Contains(playerTransform)
            )
         );

        Node weaponTree = new ConditionSplitNode(
            attackTree,
            weaponSearchTree,
            () => blackBoard.GetVariable<GameObject>(VariableNames.HELT_WEAPON_GameObject) != null
        );

        tree = new SelectorNode(weaponTree, playerDetectionTree, patrolTree);

        tree.SetupBlackboard(blackBoard);
    }

    private void Start()
    {
        ninjaBlackboard = FindObjectOfType<Ninja>().blackBoard;
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

    private void OnDrawGizmos()
    {
        Vector3 higherTransform = transform.position + Vector3.up * 2;

        // Vision
        Gizmos.color = Color.blue;
        Vector3 visionForward = transform.forward * viewRadius;
        for (float angle = -viewAngle / 2; angle < viewAngle / 2; angle += viewAngle / visualisationLines)
        {
            Gizmos.DrawLine(higherTransform, higherTransform + Quaternion.AngleAxis(angle, transform.up) * visionForward);
        }

        Vector3 visionRight = Quaternion.AngleAxis(viewAngle / 2, transform.up) * visionForward;
        Gizmos.DrawLine(higherTransform, higherTransform + visionRight);

        // Attack Range
        Gizmos.color = Color.red;
        Vector3 attackForward = transform.forward * attackRange;
        for (float angle = -attackAngle / 2; angle < attackAngle / 2; angle += attackAngle / visualisationLines)
        {
            Gizmos.DrawLine(higherTransform, higherTransform + Quaternion.AngleAxis(angle, transform.up) * attackForward);
        }

        Vector3 attackRight = Quaternion.AngleAxis(viewAngle / 2, transform.up) * attackForward;
        Gizmos.DrawLine(higherTransform, higherTransform + attackRight);
    }
}
