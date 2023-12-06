namespace BehaiviourTree
{
    public class InverterNode : DecoratorNode
    {
        public InverterNode(Node child) : base(child) { }

        public override void OnUpdate()
        {
            NodeStatus childStatus = child.Tick();

            switch (childStatus)
            {
                case NodeStatus.Success:
                    Status = NodeStatus.Failed; break;
                case NodeStatus.Failed:
                    Status = NodeStatus.Success; break;
                case NodeStatus.Running:
                    Status = NodeStatus.Running; break;
                default:
                    Status = NodeStatus.Success; break;
            }
        }
    }
}

