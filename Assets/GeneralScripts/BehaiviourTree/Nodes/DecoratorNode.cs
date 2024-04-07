namespace BehaiviourTree
{
    public abstract class DecoratorNode : Node
    {
        protected Node child;

        public DecoratorNode(Node child)
        {
            this.child = child;
        }

        public override void SetupBlackboard(BlackBoard blackboard)
        {
            base.SetupBlackboard(blackboard);
            child.SetupBlackboard(blackboard);
        }
    }
}
