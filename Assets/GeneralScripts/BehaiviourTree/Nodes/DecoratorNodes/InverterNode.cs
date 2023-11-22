namespace BehaiviourTree
{
    public class InverterNode : DecoratorNode
    {
        public InverterNode(Node child) : base(child) { }

        public override NodeStatus Evaluate()
        {
            NodeStatus childStatus = child.Tick();

            switch (childStatus)
            {
                case NodeStatus.Succes:
                    return NodeStatus.Failed;
                case NodeStatus.Failed:
                    return NodeStatus.Succes;
                case NodeStatus.Running:
                    return NodeStatus.Running;
                default:
                    return NodeStatus.Succes;
            }
        }
    }
}

