namespace BehaiviourTree
{
    public class SequenceNode : CompositeNode
    {
        private int index = 0;
        
        public SequenceNode(params Node[] childNodes) : base(childNodes) { }

        public override NodeStatus Evaluate()
        {
            for (; index < childNodes.Length; index++) 
            {
                switch (childNodes[index].Tick())
                {
                    case NodeStatus.Running: return NodeStatus.Running;
                    case NodeStatus.Failed: index = 0; return NodeStatus.Failed;
                    case NodeStatus.Succes: continue;
                }
            }

            index = 0;
            return NodeStatus.Succes;
        }
    }
}

