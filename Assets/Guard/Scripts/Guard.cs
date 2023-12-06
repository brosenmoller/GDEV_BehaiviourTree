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

    [Header("Field Of View")]
    [SerializeField] private float viewRadius;
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField, Range(2, 100)] private int visualisationLines;

    private NavMeshAgent agent;
    private Animator animator;

    private Node tree;
    private BlackBoard blackBoard;
    private Transform playerTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = FindObjectOfType<PlayerMovement>().transform;

        blackBoard = new BlackBoard();
        blackBoard.SetVariable(VariableNames.CURRENT_WAYPOINT_INDEX_Int, 0);
        blackBoard.SetVariable<GameObject>(VariableNames.HELT_WEAPON_GameObject, null);

        Node patrolTree = new SequenceNode(
            new SetTargetToNextWaypoint(wayPoints),
            new MoveToTargetPositionNode(agent, moveSpeed, stoppingDistance)
        );

        Node playerChaseTree = new SequenceNode(
            new ActionExecuterNode(() => blackBoard.SetVariable(VariableNames.TARGET_TRANSFORM, playerTransform)),
            new MoveToTargetTransformNode(agent, moveSpeed, stoppingDistance)
        );

        Node playerDetectionTree = new ResettingSequenceNode(
            new DetectObjectsNode(transform, viewRadius, viewAngle, targetMask, obstacleMask),
            new ConditionNode(
                playerChaseTree,
                () => blackBoard.GetVariable<Transform[]>(VariableNames.VISIBLE_TARGETS_TransformArray).Contains(playerTransform)
            )
        );

        Node pickupWeaponTree = new SequenceNode(
            new ActionExecuterNode(() => blackBoard.SetVariable(
                    VariableNames.TARGET_POSITION_Vec3,
                    Array.Find(
                        blackBoard.GetVariable<Transform[]>(VariableNames.VISIBLE_TARGETS_TransformArray), 
                        element => element.gameObject.CompareTag("Weapon")
                    ).position
                )
            ),
            new MoveToTargetPositionNode(agent, moveSpeed, stoppingDistance)
            // Pickup Weapon Node
        );

        Node weaponSearchTree = new ConditionNode(
             new ResettingSequenceNode(
                 new DetectObjectsNode(transform, viewRadius, 360, targetMask, obstacleMask),
                 new ConditionNode(
                    pickupWeaponTree,
                    () => Array.Exists(
                        blackBoard.GetVariable<Transform[]>(VariableNames.VISIBLE_TARGETS_TransformArray), 
                        element => element.gameObject.CompareTag("Weapon")
                    )
                )
             ),
             () => !blackBoard.GetVariable<bool>(VariableNames.SEARCHING_FOR_WEAPON_Bool) || 
                    blackBoard.GetVariable<GameObject>(VariableNames.HELT_WEAPON_GameObject) == null
        );

        tree = new SelectorNode(weaponSearchTree, playerDetectionTree, patrolTree);

        tree.SetupBlackboard(blackBoard);
    }

    private void Update()
    {
        tree.Tick();

        if (agent.isStopped)
        {
            animator.SetBool("Moving", false);
        }
        else
        {
            animator.SetBool("Moving", true);
        }

        //Transform[] visibleTransforms = blackBoard.GetVariable<Transform[]>(VariableNames.VISIBLE_TARGETS_TransformArray);
        //for (int i = 0; i < visibleTransforms.Length; i++)
        //{
        //    Debug.Log(visibleTransforms[i].gameObject.name);
        //}
    }

    private void OnDrawGizmos()
    {
        Vector3 forwardDirection = transform.forward * viewRadius;
        Vector3 higherTransform = transform.position + Vector3.up * 2;

        Gizmos.color = Color.yellow;

        for (float angle = -viewAngle / 2; angle < viewAngle / 2; angle += viewAngle / visualisationLines)
        {
            Gizmos.DrawLine(higherTransform, higherTransform + Quaternion.AngleAxis(angle, transform.up) * forwardDirection);
        }

        Vector3 right = Quaternion.AngleAxis(viewAngle / 2, transform.up) * forwardDirection;
        Gizmos.DrawLine(higherTransform, higherTransform + right);
    }
}


