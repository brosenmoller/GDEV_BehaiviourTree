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
                case NodeStatus.Succes:
                    Status = NodeStatus.Failed; break;
                case NodeStatus.Failed:
                    Status = NodeStatus.Succes; break;
                case NodeStatus.Running:
                    Status = NodeStatus.Running; break;
                default:
                    Status = NodeStatus.Succes; break;
            }
        }
    }
}

