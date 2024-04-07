namespace BehaiviourTree
{
    public abstract class CompositeNode : Node
    {
        protected readonly Node[] childNodes;

        public CompositeNode(params Node[] childNodes)
        {
            this.childNodes = childNodes;
        }

        public override void SetupBlackboard(BlackBoard blackboard)
        {
            base.SetupBlackboard(blackboard);
            foreach (Node node in childNodes)
            {
                node.SetupBlackboard(blackboard);
            }
        }
    }
}
