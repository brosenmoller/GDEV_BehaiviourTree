
namespace BehaiviourTree
{
    public class SelectorNode : CompositeNode
    {
        public SelectorNode(params Node[] childNodes) : base(childNodes) { }

        public override void OnUpdate()
        {
            for (int i = 0; i < childNodes.Length; i++)
            {
                NodeStatus childStatus = childNodes[i].Tick();
                switch (childStatus)
                {
                    case NodeStatus.Success: Status = NodeStatus.Success; return;
                    case NodeStatus.Failed: continue;
                    case NodeStatus.Running: Status =  NodeStatus.Running; return;
                }
            }
            Status = NodeStatus.Success;
        }
    }
}