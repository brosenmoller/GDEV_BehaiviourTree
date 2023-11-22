using System;

namespace BehaiviourTree
{
    public class ConditionNode : DecoratorNode
    {
        private readonly Func<bool> condition;

        public ConditionNode(Node child, Func<bool> condition) : base(child) 
        {
            this.condition = condition;
        }

        public override NodeStatus Evaluate()
        {
            if (condition())
            {
                return child.Tick();
            }
            else
            {
                return NodeStatus.Failed;
            }
        }
    }
}

