namespace BehaiviourTree
{
    public abstract class Node
    {
        public NodeStatus Status = NodeStatus.Uninitialised;

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }

        public NodeStatus Tick()
        {
            NodeStatus oldStatus = Status;
            Status = Evaluate();

            if (oldStatus == NodeStatus.Uninitialised)
            {
                switch (Status)
                {
                    case NodeStatus.Running:
                        OnEnter(); break;
                    case NodeStatus.Failed:
                        return NodeStatus.Failed;
                    case NodeStatus.Succes:
                        return NodeStatus.Succes;
                }
            }

            switch (Status)
            {
                case NodeStatus.Running:
                    OnUpdate();
                    return NodeStatus.Running;
                case NodeStatus.Failed:
                    Reset();
                    return NodeStatus.Failed;
                case NodeStatus.Succes:
                    Reset();
                    return NodeStatus.Succes;
                default:
                    return NodeStatus.Failed;
            }
        }

        private void Reset()
        {
            Status = NodeStatus.Uninitialised;
            OnExit();
        }

        public abstract NodeStatus Evaluate();
    }
}


