namespace BehaiviourTree
{
    public class SequenceNode : CompositeNode
    {
        private int index = 0;
        
        public SequenceNode(params Node[] childNodes) : base(childNodes) { }

        public override void OnUpdate()
        {
            for (; index < childNodes.Length; index++) 
            {
                switch (childNodes[index].Tick())
                {
                    case NodeStatus.Running: Status = NodeStatus.Running; return;
                    case NodeStatus.Failed: index = 0; Status = NodeStatus.Failed; return;
                    case NodeStatus.Success: continue;
                }
            }

            index = 0;
            Status = NodeStatus.Success;
        }
    }
}

