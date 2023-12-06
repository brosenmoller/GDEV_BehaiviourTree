namespace BehaiviourTree
{
    public class ResettingSequenceNode : CompositeNode
    {
        public ResettingSequenceNode(params Node[] childNodes) : base(childNodes) { }

        public override void OnUpdate()
        {
            for (int index = 0; index < childNodes.Length; index++)
            {
                switch (childNodes[index].Tick())
                {
                    case NodeStatus.Running: Status = NodeStatus.Running; return;
                    case NodeStatus.Failed: Status = NodeStatus.Failed; return;
                    case NodeStatus.Success: continue;
                }
            }

            Status = NodeStatus.Success;
        }
    }
}