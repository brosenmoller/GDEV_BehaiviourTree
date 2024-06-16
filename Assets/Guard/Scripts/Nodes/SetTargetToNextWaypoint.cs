using BehaiviourTree;
using UnityEngine;

public class SetTargetToNextWaypoint : Node
{
    private readonly Transform[] wayPoints;
    public SetTargetToNextWaypoint(Transform[] wayPoints)
    {
        this.wayPoints = wayPoints;
    }

    public override void OnEnter()
    {
        Debug.Log("Set Waypoint");

        int currentIndex = blackboard.GetVariable<int>(VariableNames.CURRENT_WAYPOINT_INDEX_Int);
        
        currentIndex++;
        
        if (currentIndex >= wayPoints.Length)
        {
            currentIndex = 0;
        }
        
        blackboard.SetVariable(VariableNames.CURRENT_WAYPOINT_INDEX_Int, currentIndex);
        blackboard.SetVariable(VariableNames.TARGET_POSITION_Vec3, wayPoints[currentIndex].position);
        
        Status = NodeStatus.Success;
    }
}
