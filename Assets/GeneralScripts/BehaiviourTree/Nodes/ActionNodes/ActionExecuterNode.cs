
using System;

namespace BehaiviourTree
{
    public class ActionExecuterNode : Node
    {
        private readonly Action action;

        public ActionExecuterNode(Action action) 
        {
            this.action = action;
        }

        public override void OnEnter()
        {
            action?.Invoke();

            Status = NodeStatus.Success;
        }
    }
}


