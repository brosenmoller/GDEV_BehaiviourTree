
using UnityEngine;

namespace BehaiviourTree
{
    public class DebugNode : Node
    {
        private readonly string debugText;

        public DebugNode(string debugText) 
        {
            this.debugText = debugText;
        }

        public override void OnEnter()
        {
            Debug.Log("ON ENTER: " + debugText);
            Status = NodeStatus.Success;
        }

        //public override void OnUpdate()
        //{
        //    Debug.Log("ON UPDATE: " + debugText);
        //}

        public override void OnExit()
        {
            Debug.Log("ON EXIT: " + debugText);
        }
    }
}
