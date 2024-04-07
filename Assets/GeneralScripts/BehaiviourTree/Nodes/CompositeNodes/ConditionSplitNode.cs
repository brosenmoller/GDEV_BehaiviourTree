using System;

namespace BehaiviourTree
{
    public class ConditionSplitNode : CompositeNode
    {
        private readonly Node ifChild;
        private readonly Node elseChild;

        private readonly Func<bool> condition;

        public ConditionSplitNode(Node ifChild, Node elseChild, Func<bool> condition) : base(ifChild, elseChild)
        {
            this.ifChild = ifChild;
            this.elseChild = elseChild;
            this.condition = condition;
        }

        public override void OnUpdate()
        {
            if (condition())
            {
                Status = ifChild.Tick();
            }
            else
            {
                Status = elseChild.Tick();
            }
        }
    }
}
